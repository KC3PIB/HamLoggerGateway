using HamLoggerGateway.MessageProcessors.N1MM.Schema;

namespace HamLoggerGateway.MessageProcessors.N1MM.Validators;

/// <summary>
///     Validates <see cref="ContactInfo" /> messages to ensure they contain the minimum required data.
/// </summary>
public class ContactInfoHasBareMinimumValidator : IMessageValidator<ContactInfo>
{
    /// <summary>
    ///     Determines whether the specified <see cref="ContactInfo" /> message is valid.
    /// </summary>
    /// <param name="message">The <see cref="ContactInfo" /> message to validate.</param>
    /// <returns>
    ///     <c>true</c> if the message is valid; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     A valid <see cref="ContactInfo" /> message must meet the following criteria:
    ///     <list type="bullet">
    ///         <item>The <see cref="ContactInfo.Call" /> property must not be null or empty.</item>
    ///         <item>
    ///             Either the <see cref="ContactInfo.StationName" /> or <see cref="ContactInfo.Operator" /> property must
    ///             not be null or empty.
    ///         </item>
    ///         <item>
    ///             The <see cref="ContactInfo.Timestamp" /> property must not be the default
    ///             <see cref="DateTime.MinValue" /> value.
    ///         </item>
    ///         <item>The <see cref="ContactInfo.Mode" /> property must not be null or empty.</item>
    ///     </list>
    /// </remarks>
    public bool IsValid(ContactInfo? message)
    {
        if (message == null) return false; // Explicit null check

        return !string.IsNullOrWhiteSpace(message.Call) &&
               (!string.IsNullOrWhiteSpace(message.StationName) || !string.IsNullOrWhiteSpace(message.Operator)) &&
               message.Timestamp != DateTime.MinValue &&
               !string.IsNullOrWhiteSpace(message.Mode);
    }

    /// <summary>
    ///     Determines whether the specified object is a valid <see cref="ContactInfo" /> message.
    /// </summary>
    /// <param name="message">The object to validate.</param>
    /// <returns>
    ///     <c>true</c> if the object is a valid <see cref="ContactInfo" /> message; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValid(object? message)
    {
        return IsValid(message as ContactInfo);
    }
}