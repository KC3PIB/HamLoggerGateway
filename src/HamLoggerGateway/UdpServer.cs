using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using HamLoggerGateway.Utils;
using Microsoft.Extensions.Logging;

namespace HamLoggerGateway;

/// <summary>
///     Represents a UDP server that handles incoming datagram messages and applies rate limiting based on IP addresses.
/// </summary>
public class UdpServer : ServerBase
{
    /// <summary>
    ///     Time after which an inactive rate limiter will be removed (15 minutes).
    /// </summary>
    private readonly TimeSpan _rateLimiterExpiry = TimeSpan.FromMinutes(15);

    /// <summary>
    ///     Stores rate limiters for each IP address.
    /// </summary>
    private readonly ConcurrentDictionary<IPAddress, RateLimiter> _rateLimiters = new();

    /// <summary>
    ///     The maximum number of requests per minute allowed from a single IP address.
    /// </summary>
    private readonly int _requestsPerMinutePerIp;

    /// <summary>
    ///     Initializes a new instance of the UdpServer using specified server settings.
    /// </summary>
    /// <param name="messageProcessor">The processor to handle incoming messages.</param>
    /// <param name="settings">Configuration settings for the server including address, port, and buffer size.</param>
    /// <param name="logger">Logger to record log entries</param>
    public UdpServer(IMessageProcessor messageProcessor, ServerSettings settings, ILogger logger) : base(
        SocketType.Dgram,
        ProtocolType.Udp, messageProcessor, settings, logger)
    {
        _requestsPerMinutePerIp = settings.RequestsPerMinutePerIp;
        Task.Run(CleanupRateLimitersAsync); // Start the cleanup task in the background
    }

    /// <summary>
    ///     Determines if the given IP address is currently rate-limited.
    /// </summary>
    /// <param name="ipAddress">The IP address to check for rate limiting.</param>
    /// <returns>True if the IP address is rate-limited, otherwise false.</returns>
    private bool IsRateLimited(IPAddress ipAddress)
    {
        // Get or add a rate limiter for the IP address
        var rateLimiter = _rateLimiters.GetOrAdd(ipAddress,
            _ => new RateLimiter(_requestsPerMinutePerIp, TimeSpan.FromMinutes(1)));
        return !rateLimiter.AllowRequest();
    }

    /// <summary>
    ///     Periodically cleans up expired rate limiters that have not been accessed within the expiry interval.
    /// </summary>
    /// <returns>A task that represents the asynchronous clean-up operation.</returns>
    private async Task CleanupRateLimitersAsync()
    {
        while (!IsDisposed)
        {
            await Task.Delay(TimeSpan.FromMinutes(5)); // Clean up every 5 minutes
            var now = DateTime.UtcNow;

            // Find expired rate limiters
            var keysToRemove = (from kvp in _rateLimiters
                where now - kvp.Value.LastAccessed > _rateLimiterExpiry
                select kvp.Key).ToList();

            // Remove expired rate limiters
            foreach (var key in keysToRemove)
                _rateLimiters.TryRemove(key, out _);
        }
    }

    /// <summary>
    ///     Continuously handles incoming messages until cancellation is requested.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the ongoing operation.</returns>
    protected override async Task HandleMessageLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var datagramBuffer = MemoryPool.Rent(BufferSize);
            try
            {
                // Receive datagram from any source
                var result = await Socket.ReceiveFromAsync(datagramBuffer.Memory[..BufferSize], SocketFlags.None,
                    LocalEndpoint,
                    cancellationToken);
                var remoteEndpoint = result.RemoteEndPoint as IPEndPoint;
                Logger.LogDebug("Datagram received from {Address}:{Port}", remoteEndpoint?.Address,
                    remoteEndpoint?.Port);

                // Input validation checks
                if (remoteEndpoint is null)
                {
                    Logger.LogWarning("Datagram received with no endpoint");
                    continue;
                }

                if (HostBlackList.IsBlacklisted(remoteEndpoint, out var reason))
                {
                    Logger.LogWarning("The host {Address} is on the blacklist {Service}",
                        remoteEndpoint.Address,
                        reason);
                    continue;
                }

                if (IsRateLimited(remoteEndpoint.Address))
                {
                    Logger.LogWarning("Datagram rate limit of {RequestsPerMinutePerIp} exceeded for {Address}",
                        _requestsPerMinutePerIp, remoteEndpoint.Address);
                    continue;
                }

                if (result.ReceivedBytes <= 0)
                {
                    Logger.LogWarning("Datagram from {Address}:{Port} was 0 bytes", remoteEndpoint.Address,
                        remoteEndpoint.Port);
                    continue;
                }

                if (result.ReceivedBytes > datagramBuffer.Memory.Length)
                {
                    Logger.LogWarning(
                        "Datagram from {Address}:{Port} was {ReceivedBytes} bytes which exceeds rented buffer size of {BufferSize}",
                        remoteEndpoint.Address,
                        remoteEndpoint.Port, result.ReceivedBytes, datagramBuffer.Memory.Length);
                    continue;
                }

                // Copy just the received bytes to a new buffer
                var message = MemoryPool.Rent(result.ReceivedBytes);
                datagramBuffer.Memory[..result.ReceivedBytes].CopyTo(message.Memory);

                // Process the message asynchronously
                _ = ProcessMessageAndCleanupBufferAsync(message, result.ReceivedBytes, remoteEndpoint,
                    cancellationToken).ContinueWith(
                    task => Logger.LogError(task.Exception, "Error processing datagram message from {Address}:{Port}",
                        remoteEndpoint.Address,
                        remoteEndpoint.Port),
                    TaskContinuationOptions.OnlyOnFaulted); // Log only if there's a fault
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellationToken is cancelled
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(exception =>
                {
                    if (exception is SocketException)
                    {
                        Logger.LogError(exception, "Socket Exception");
                        return true;
                    }

                    return false;
                });
            }
        }
    }
}