using System.Buffers;
using System.Net;
using System.Net.Sockets;
using HamLoggerGateway.Utils;
using Microsoft.Extensions.Logging;

namespace HamLoggerGateway;

/// <summary>
///     Represents a TCP server that handles incoming TCP connections asynchronously, processes received data, and manages
///     network communications efficiently.
/// </summary>
public class TcpServer : ServerBase
{
    /// <summary>
    ///     Initializes a new instance of the TcpServer with specified settings.
    /// </summary>
    /// <param name="messageProcessor">The processor to handle messages received from clients.</param>
    /// <param name="settings">Configuration settings including the network address and port.</param>
    /// <param name="logger">Logger to record log entries</param>
    public TcpServer(IMessageProcessor messageProcessor, ServerSettings settings, ILogger logger)
        : base(SocketType.Stream, ProtocolType.Tcp, messageProcessor, settings, logger)
    {
    }

    /// <summary>
    ///     Asynchronously listens for client connections and processes each connection in a separate task.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    protected override async Task HandleMessageLoopAsync(CancellationToken cancellationToken)
    {
        Socket.Listen();

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                var clientSocket = await Socket.AcceptAsync(cancellationToken);

                // Start a new task to handle the client connection
                _ = HandleClientAsync(clientSocket, cancellationToken).ContinueWith(
                    task => Logger.LogError(task.Exception, "Error in handling client connection"),
                    TaskContinuationOptions.OnlyOnFaulted); // Log only if there's a fault
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellationToken is cancelled
            }
            catch (SocketException socketException)
            {
                // Handle specific socket exceptions
                Logger.LogError(socketException, "Socket Exception while accepting a client");
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                Logger.LogError(objectDisposedException, "Object disposed while accessing socket");
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(exception =>
                {
                    if (exception is SocketException)
                    {
                        Logger.LogError(exception, "Socket Exception while accepting a client");
                        return true; // The exception was handled
                    }

                    return false; // The exception was not handled
                });
            }
    }


    /// <summary>
    ///     Handles individual client connections, reads data from the client, and processes it asynchronously.
    /// </summary>
    /// <param name="clientSocket">The client socket.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    private async Task HandleClientAsync(Socket clientSocket, CancellationToken cancellationToken)
    {
        using var owner = MemoryPool.Rent(BufferSize);
        var remoteEndpoint = clientSocket.RemoteEndPoint as IPEndPoint;

        // Ensure we have an endpoint for the client
        if (remoteEndpoint == null)
        {
            Logger.LogWarning("Network stream received with no endpoint");
            return;
        }

        Logger.LogDebug("Network stream received from {Address}:{Port}", remoteEndpoint.Address,
            remoteEndpoint.Port);

        // Check if the IP address is blacklisted
        if (HostBlackList.IsBlacklisted(remoteEndpoint, out var reason))
        {
            Logger.LogWarning("The host {Address} is on the blacklist {Service}",
                remoteEndpoint.Address,
                reason);
            return;
        }

        await using var clientStream = new NetworkStream(clientSocket, true);

        // Read data from the network stream until the buffer is full or cancellation is requested.
        // The data is read into the rented memory buffer `owner`.
        var totalBytes = 0;
        int numOfBytesRead;
        while ((numOfBytesRead = await clientStream.ReadAsync(owner.Memory, cancellationToken)) > 0 &&
               totalBytes < owner.Memory.Length)
            totalBytes += numOfBytesRead;

        // If the total bytes exceed the buffer size, simply stop reading and process received data
        if (totalBytes >= owner.Memory.Length)
            Logger.LogWarning(
                "Network stream from {Address}:{Port} received {ReceivedBytes} bytes which exceeds rented buffer size of {BufferSize}",
                remoteEndpoint.Address,
                remoteEndpoint.Port, totalBytes, owner.Memory.Length);

        // Process the received data if there is any
        if (totalBytes > 0)
        {
            // Create a new memory buffer to store the received data.
            // The data is copied from the rented memory buffer `owner` to the new buffer `message`.
            var message = MemoryPool<byte>.Shared.Rent(totalBytes);
            owner.Memory[..totalBytes].CopyTo(message.Memory);

            // Process the message and clean up the memory pool.
            _ = ProcessMessageAndCleanupBufferAsync(message, totalBytes, remoteEndpoint, cancellationToken);
        }
        else
        {
            // Log a warning if no data was read from the network stream.
            Logger.LogWarning("Network stream from {Address}:{Port} read 0 bytes", remoteEndpoint.Address,
                remoteEndpoint.Port);
        }
    }
}