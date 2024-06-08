using System.Net;
using HamLoggerGateway.MessageProcessors.N1MM.Schema;

namespace HamLoggerGateway.MessageProcessors.N1MM;

/// <summary>
///     Defines an interface for handling N1MM UDP messages.
/// </summary>
public interface IMessageHandlerN1MM
{
    /// <summary>
    ///     Asynchronously handles an <see cref="message" /> message.
    /// </summary>
    /// <param name="message">The <see cref="endPoint" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleAppInfoAsync(AppInfo message, EndPoint endPoint, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="ContactInfo" /> message.
    /// </summary>
    /// <param name="message">The <see cref="ContactInfo" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleContactInfoAsync(ContactInfo message, EndPoint endPoint, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="ContactReplace" /> message.
    /// </summary>
    /// <param name="message">The <see cref="ContactReplace" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleContactReplaceAsync(ContactReplace message, EndPoint endPoint,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="ContactDelete" /> message.
    /// </summary>
    /// <param name="message">The <see cref="ContactDelete" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleContactDeleteAsync(ContactDelete message, EndPoint endPoint,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="LookupInfo" /> message.
    /// </summary>
    /// <param name="message">The <see cref="LookupInfo" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleLookupInfoAsync(LookupInfo message, EndPoint endPoint, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="Spot" /> message.
    /// </summary>
    /// <param name="message">The <see cref="Spot" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleSpotAsync(Spot message, EndPoint endPoint, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="DynamicResults" /> message.
    /// </summary>
    /// <param name="message">The <see cref="DynamicResults" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleDynamicResultsAsync(DynamicResults message, EndPoint endPoint,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously handles a <see cref="RadioInfo" /> message.
    /// </summary>
    /// <param name="message">The <see cref="RadioInfo" /> message to handle.</param>
    /// <param name="endPoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleRadioInfoAsync(RadioInfo message, EndPoint endPoint, CancellationToken cancellationToken = default);
}