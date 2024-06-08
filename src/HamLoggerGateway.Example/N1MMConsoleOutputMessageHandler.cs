using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using HamLoggerGateway.MessageProcessors.N1MM;
using HamLoggerGateway.MessageProcessors.N1MM.Schema;

namespace HamLoggerGateway.Example;

/// <summary>
/// Handles incoming N1MM messages by writing them to the console in JSON format. 
/// This serves as a simple example of how to process N1MM messages within the HamLoggerGateway.
/// </summary>
public class N1MMConsoleOutputMessageHandler : IMessageHandlerN1MM
{
    /// <summary>
    /// Handles the `AppInfo` message, which contains information about the N1MM application.
    /// </summary>
    /// <param name="message">The `AppInfo` message object containing details like version, name, etc.</param>
    /// <param name="endPoint">The network endpoint (IP address and port) from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleAppInfoAsync(AppInfo message, EndPoint endPoint, CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `ContactInfo` message, providing details about a contact (e.g., callsign, frequency, mode).
    /// </summary>
    /// <param name="message">The `ContactInfo` message object.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleContactInfoAsync(ContactInfo message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `ContactReplace` message, indicating a replacement for an existing contact entry.
    /// </summary>
    /// <param name="message">The `ContactReplace` message object.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleContactReplaceAsync(ContactReplace message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `ContactDelete` message, indicating the deletion of a contact from the log.
    /// </summary>
    /// <param name="message">The `ContactDelete` message object containing the ID of the deleted contact.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleContactDeleteAsync(ContactDelete message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `LookupInfo` message, providing information about a callsign lookup (e.g., name, QTH).
    /// </summary>
    /// <param name="message">The `LookupInfo` message object.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleLookupInfoAsync(LookupInfo message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `Spot` message, representing a DX spot received by the logging software.
    /// </summary>
    /// <param name="message">The `Spot` message object containing details like frequency, mode, callsign, etc.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleSpotAsync(Spot message, EndPoint endPoint, CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `DynamicResults` message, containing results from a contest or other dynamic activity.
    /// </summary>
    /// <param name="message">The `DynamicResults` message object.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleDynamicResultsAsync(DynamicResults message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the `RadioInfo` message, providing information about the current state of the radio transceiver.
    /// </summary>
    /// <param name="message">The `RadioInfo` message object containing details like frequency, mode, power, etc.</param>
    /// <param name="endPoint">The network endpoint from which the message was received.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task HandleRadioInfoAsync(RadioInfo message, EndPoint endPoint,
        CancellationToken cancellationToken = default)
    {
        WriteMessageAsJsonToConsole(message);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Writes a message object to the console in formatted JSON.
    /// </summary>
    /// <remarks>
    /// This method uses `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` for web compatibility, and `WriteIndented` for readability.
    /// </remarks>
    /// <param name="message">The object to serialize and write to the console.</param>
    private static void WriteMessageAsJsonToConsole(object message)
    {
        Console.WriteLine(JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        }));
    }
}