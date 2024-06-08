namespace HamLoggerGateway;

/// <summary>
///     Defines a base interface for validating messages.
/// </summary>
public interface IMessageValidator
{
    /// <summary>
    ///     Determines whether the specified message is valid.
    /// </summary>
    /// <param name="message">The message to validate.</param>
    /// <returns>
    ///     <c>true</c> if the message is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValid(object? message);
}

/// <summary>
///     Defines a generic interface for validating messages of a specific type.
/// </summary>
/// <typeparam name="T">The type of message to validate.</typeparam>
public interface IMessageValidator<in T> : IMessageValidator
{
    /// <summary>
    ///     Determines whether the specified message of type <typeparamref name="T" /> is valid.
    /// </summary>
    /// <param name="message">The message to validate.</param>
    /// <returns>
    ///     <c>true</c> if the message is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValid(T message);
}