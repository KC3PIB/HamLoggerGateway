using System.Buffers;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace HamLoggerGateway;

/// <summary>
///     Provides a base class for implementing network servers.
/// </summary>
public abstract class ServerBase
{
    /// <summary>
    ///     Default size of a UDP packet buffer (1500 bytes).
    /// </summary>
    private const int DefaultDatagramBufferSize = 1500;

    /// <summary>
    ///     Default size of a TCP packet buffer (16384 bytes).
    /// </summary>
    private const int DefaultTcpStreamBufferSize = 16384;

    /// <summary>
    ///     Locking object used for synchronization to ensure thread safety.
    /// </summary>
    private readonly object _syncRoot = new();

    /// <summary>
    ///     The size of the datagram buffers in bytes.
    /// </summary>
    protected readonly int BufferSize;

    /// <summary>
    ///     The local endpoint to which the server is bound.
    /// </summary>
    protected readonly IPEndPoint LocalEndpoint;

    /// <summary>
    ///     Logger for recording events and errors.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    ///     Memory pool for efficient allocation of datagram and message buffers.
    /// </summary>
    protected readonly MemoryPool<byte> MemoryPool = MemoryPool<byte>.Shared;

    /// <summary>
    ///     The underlying socket used for network communication.
    /// </summary>
    protected Socket Socket;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServerBase" /> class.
    /// </summary>
    /// <param name="socketType">The type of socket to use (e.g., UDP or TCP).</param>
    /// <param name="protocolType">The protocol to use (e.g., UDP or TCP).</param>
    /// <param name="messageProcessor">The message processor for handling incoming messages.</param>
    /// <param name="settings">Configuration settings for the server.</param>
    /// <param name="logger">Logger for recording events and errors.</param>
    /// <exception cref="ArgumentException">Thrown when the IP address or port number in the settings is invalid.</exception>
    protected ServerBase(SocketType socketType,
        ProtocolType protocolType,
        IMessageProcessor messageProcessor,
        ServerSettings settings,
        ILogger logger)
    {
        if (!IPAddress.TryParse(settings.Address, out var address))
            throw new ArgumentException("Invalid IP address.", nameof(settings));

        if (settings.Port is < 1 or > 65535)
            throw new ArgumentException("Port number must be between 1 and 65535.", nameof(settings));

        Logger = logger;

        // setup server endpoint
        LocalEndpoint = new IPEndPoint(address, settings.Port);

        // size the buffers and assign the message processor
        BufferSize = settings.BufferSize is > 0 ? settings.BufferSize.Value :
            socketType == SocketType.Dgram ? DefaultDatagramBufferSize : DefaultTcpStreamBufferSize;
        MessageProcessor = messageProcessor;

        // setup socket allowing for shared addresses
        Socket = new Socket(socketType, protocolType);
        if (settings.EnableReuseAddress)
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        Socket.Bind(LocalEndpoint);
    }

    /// <summary>
    ///     Source for cancellation tokens used to stop the server.
    /// </summary>
    private CancellationTokenSource? CancellationTokenSource { get; set; }

    /// <summary>
    ///     The task that handles the message loop.
    /// </summary>
    private Task? MessageLoopTask { get; set; }

    /// <summary>
    ///     The message processor responsible for handling incoming messages.
    /// </summary>
    private IMessageProcessor MessageProcessor { get; }

    /// <summary>
    ///     Indicates whether this object has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    ///     Indicates whether the server is currently running.
    /// </summary>
    protected bool IsRunning { get; private set; }

    /// <summary>
    ///     Finalizer to ensure disposal in case Dispose() is not called explicitly.
    /// </summary>
    ~ServerBase()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Starts the server with an optional cancellation token source.
    /// </summary>
    /// <param name="cancellationTokenSource">The cancellation token source to use, or null to create a new one.</param>
    /// <exception cref="InvalidOperationException">Thrown if the server is already running.</exception>
    public void Start(CancellationTokenSource? cancellationTokenSource = default)
    {
        lock (_syncRoot) // Locking to ensure thread safety
        {
            if (IsRunning)
                throw new InvalidOperationException("The server is already running.");

            IsRunning = true;
        }

        CancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
        MessageLoopTask = HandleMessageLoopAsync(CancellationTokenSource.Token);
    }

    /// <summary>
    ///     Stops the server and terminates the message handling.
    /// </summary>
    public void Stop()
    {
        lock (_syncRoot) // Locking to ensure thread safety
        {
            if (!IsRunning)
                throw new InvalidOperationException("The server is not running.");

            try
            {
                CancellationTokenSource?.Cancel();
                MessageLoopTask?.Wait(1000);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(ex => ex is TaskCanceledException);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }

    /// <summary>
    ///     Processes a received message and cleans up the buffer.
    /// </summary>
    /// <param name="data">The memory buffer containing the message data.</param>
    /// <param name="size">The size of the data in bytes.</param>
    /// <param name="remoteEndpoint">The endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task ProcessMessageAndCleanupBufferAsync(IMemoryOwner<byte> data, int size,
        IPEndPoint remoteEndpoint,
        CancellationToken cancellationToken)
    {
        try
        {
            await MessageProcessor.ProcessMessageAsync(data.Memory[..size], remoteEndpoint, cancellationToken);
        }
        finally
        {
            data.Dispose();
        }
    }

    /// <summary>
    ///     When overridden in a derived class, handles the message loop asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task HandleMessageLoopAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Releases all resources used by the ServerBase.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the ServerBase and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        if (disposing)
        {
            // Dispose managed resources.
            CancellationTokenSource?.Dispose();
            CancellationTokenSource = null;
            try
            {
                Socket.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Socket Exception on Dispose");
            }
        }

        IsDisposed = true;
    }
}