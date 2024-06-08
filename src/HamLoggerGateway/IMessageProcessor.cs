using System.Net;

namespace HamLoggerGateway;

/// <summary>
///     Defines an interface for processing incoming messages asynchronously.
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    ///     Asynchronously processes the given message data from a remote endpoint.
    /// </summary>
    /// <param name="data">A memory segment containing the message data.</param>
    /// <param name="remoteEndpoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous processing operation.</returns>
    Task ProcessMessageAsync(Memory<byte> data, IPEndPoint remoteEndpoint,
        CancellationToken cancellationToken = default);
}