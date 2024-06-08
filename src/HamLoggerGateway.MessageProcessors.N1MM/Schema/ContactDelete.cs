using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents a packet for deleting a contact, containing minimal information
///     necessary to identify and remove a specific contact from the log.
/// </summary>
[XmlRoot("contactdelete")]
public class ContactDelete
{
    /// <summary>Gets or sets the application name, usually "N1MM".</summary>
    [XmlElement("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the timestamp of the contact. The format is expected to be "YYYY-MM-DD HH:MM:SS".
    /// </summary>
    [XmlIgnore]
    public DateTime Timestamp { get; set; }


    [XmlElement("timestamp")]
    public string TimestampString
    {
        get => Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
        set
        {
            if (!N1MMDateTimeUtils.TryParseN1MMDateTime(value, out var parsedDate))
                throw new InvalidOperationException($"Invalid datetime value '{value}' for Timestamp.");
            Timestamp = parsedDate;
        }
    }

    /// <summary>Gets or sets the callsign associated with the contact to be deleted.</summary>
    [XmlElement("call")]
    public string Call { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the contest number. This number is unique to the contest
    ///     within the database and helps identify the specific contest instance.
    /// </summary>
    [XmlElement("contestnr")]
    public int ContestNr { get; set; }

    /// <summary>Gets or sets the station name, indicating where the contact was logged.</summary>
    [XmlElement("StationName")]
    public string StationName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the ID of the contact to be deleted. This is a unique identifier
    ///     for each contact in the log, facilitating precise deletion.
    /// </summary>
    [XmlElement("ID")]
    public string Id { get; set; } = string.Empty;
}