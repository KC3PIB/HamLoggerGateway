using System.Globalization;

namespace HamLoggerGateway.MessageProcessors.N1MM;

/// <summary>
///     Provides utility methods for working with date and time formats in the context of N1MM messages.
/// </summary>
public static class N1MMDateTimeUtils
{
    /// <summary>
    ///     Gets an array of supported date and time formats used in N1MM messages.
    /// </summary>
    /// <remarks>
    ///     These formats are typically used for parsing and formatting timestamps within the XML payloads of N1MM messages.
    /// </remarks>
    private static readonly string[] DateFormats =
    [
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss"
    ];

    /// <summary>
    ///     Parses a string representation of a date and time using the supported N1MM date formats.
    /// </summary>
    /// <param name="dateTimeString">The string to parse.</param>
    /// <param name="parsedDateTime">
    ///     The parsed <see cref="DateTime" /> value, or <see cref="DateTime.MinValue" /> if parsing
    ///     fails.
    /// </param>
    /// <returns>True if parsing succeeds; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dateTimeString" /> is null.</exception>
    public static bool TryParseN1MMDateTime(string dateTimeString, out DateTime parsedDateTime)
    {
        ArgumentNullException.ThrowIfNull(dateTimeString);
        return DateTime.TryParseExact(dateTimeString, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out parsedDateTime);
    }
}