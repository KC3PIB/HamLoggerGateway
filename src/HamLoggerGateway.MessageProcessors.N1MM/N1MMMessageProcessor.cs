using System.Net;
using System.Xml;
using System.Xml.Serialization;
using HamLoggerGateway.MessageProcessors.N1MM.Schema;
using HamLoggerGateway.MessageProcessors.N1MM.Validators;
using Microsoft.Extensions.Logging;

namespace HamLoggerGateway.MessageProcessors.N1MM;

/// <summary>
///     Processes N1MM UDP messages by deserializing, validating, and handling them.
/// </summary>
public class N1MMMessageProcessor : IMessageProcessor
{
    private readonly ILogger _logger;
    private readonly IMessageHandlerN1MM _messageHandler;
    private readonly Dictionary<string, Type> _messageTypes;
    private readonly Dictionary<Type, IMessageValidator> _validators;

    /// <summary>
    ///     Initializes a new instance of the <see cref="N1MMMessageProcessor" /> class with a default validator for
    ///     <see cref="ContactInfo" />.
    /// </summary>
    /// <param name="messageHandler">The handler for processed and validated N1MM messages.</param>
    /// <param name="logger">The logger for recording events and errors.</param>
    public N1MMMessageProcessor(IMessageHandlerN1MM messageHandler, ILogger logger)
        : this(messageHandler, new Dictionary<Type, IMessageValidator>
        {
            { typeof(ContactInfo), new ContactInfoHasBareMinimumValidator() }
        }, logger)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="N1MMMessageProcessor" /> class.
    /// </summary>
    /// <param name="messageHandler">The handler for processed and validated N1MM messages.</param>
    /// <param name="messageValidators">A dictionary of validators for different message types.</param>
    /// <param name="logger">The logger for recording events and errors.</param>
    public N1MMMessageProcessor(IMessageHandlerN1MM messageHandler,
        Dictionary<Type, IMessageValidator> messageValidators, ILogger logger)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _validators = messageValidators;
        _messageTypes = new Dictionary<string, Type>
        {
            { "appinfo", typeof(AppInfo) },
            { "contactinfo", typeof(ContactInfo) },
            { "contactreplace", typeof(ContactReplace) },
            { "contactdelete", typeof(ContactDelete) },
            { "lookupinfo", typeof(LookupInfo) },
            { "spot", typeof(Spot) },
            { "dynamicresults", typeof(DynamicResults) },
            { "radioinfo", typeof(RadioInfo) }
        };
    }

    /// <summary>
    ///     Asynchronously processes the given message data from a remote endpoint.
    /// </summary>
    /// <param name="data">A memory segment containing the message data.</param>
    /// <param name="remoteEndpoint">The endpoint from which the message originated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous processing operation.</returns>
    public async Task ProcessMessageAsync(Memory<byte> data, IPEndPoint remoteEndpoint,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var stream = new MemoryStream(data.ToArray());
            using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });

            await reader.MoveToContentAsync();
            var rootElementName = reader.Name.ToLowerInvariant();

            if (!_messageTypes.TryGetValue(rootElementName, out var messageType))
            {
                _logger.LogWarning("Unknown message type: {RootElementName}", rootElementName);
                return;
            }

            stream.Position = 0; // Reset stream for deserialization
            var serializer = new XmlSerializer(messageType, new XmlAttributeOverrides());
            var messageObject = serializer.Deserialize(stream);

            if (messageObject == null)
            {
                _logger.LogWarning("Null message received for type: {MessageType}", messageType);
                return;
            }

            if (_validators.TryGetValue(messageType, out var validator) && !validator.IsValid(messageObject))
            {
                _logger.LogWarning("Invalid message received: {MessageObject}", messageObject);
                return;
            }

            await ProcessValidMessageAsync(messageObject, messageType, remoteEndpoint, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing N1MM message");
        }
    }

    /// <summary>
    ///     Asynchronously processes a valid N1MM message of the given type.
    /// </summary>
    /// <param name="message">The message object.</param>
    /// <param name="messageType">The type of the message.</param>
    /// <param name="remoteEndpoint">The remote endpoint from which the message was received.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ProcessValidMessageAsync(object message, Type messageType, IPEndPoint remoteEndpoint,
        CancellationToken cancellationToken)
    {
        // Use a switch expression for concise type matching
        switch (message)
        {
            case AppInfo appInfo:
                await _messageHandler.HandleAppInfoAsync(appInfo, remoteEndpoint, cancellationToken);
                break;
            case ContactDelete contactDelete:
                await _messageHandler.HandleContactDeleteAsync(contactDelete, remoteEndpoint, cancellationToken);
                break;
            case LookupInfo lookupInfo:
                await _messageHandler.HandleLookupInfoAsync(lookupInfo, remoteEndpoint, cancellationToken);
                break;
            case ContactReplace contactReplace:
                await _messageHandler.HandleContactReplaceAsync(contactReplace, remoteEndpoint, cancellationToken);
                break;
            case ContactInfo contactInfo:
                await _messageHandler.HandleContactInfoAsync(contactInfo, remoteEndpoint, cancellationToken);
                break;
            case Spot spot:
                await _messageHandler.HandleSpotAsync(spot, remoteEndpoint, cancellationToken);
                break;
            case DynamicResults dynamicResults:
                await _messageHandler.HandleDynamicResultsAsync(dynamicResults, remoteEndpoint, cancellationToken);
                break;
            case RadioInfo radioInfo:
                await _messageHandler.HandleRadioInfoAsync(radioInfo, remoteEndpoint, cancellationToken);
                break;
            default:
                _logger.LogWarning("Unhandled message type: {MessageType}", messageType);
                break;
        }
    }
}