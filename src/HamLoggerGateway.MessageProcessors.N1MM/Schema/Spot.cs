using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents a spot message sent via UDP, containing information about a spotted station.
///     This includes details such as the frequency, callsign, mode, and timestamp of the spot.
/// </summary>
[XmlRoot("spot")]
public class Spot
{
    /// <summary>
    ///     Gets or sets the application name, usually "N1MM".
    /// </summary>
    [XmlElement("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the station name, typically the NetBIOS name of the computer sending the message.
    /// </summary>
    [XmlElement("StationName")]
    public string StationName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the callsign of the spotted station.
    /// </summary>
    [XmlElement("dxcall")]
    public string DxCall { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the frequency of the spot in Hertz.
    /// </summary>
    [XmlElement("frequency")]
    public double Frequency { get; set; }

    /// <summary>
    ///     Gets or sets the callsign of the station that spotted the call.
    /// </summary>
    [XmlElement("spottercall")]
    public string SpotterCall { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the timestamp of the spot. The format is expected to be "YYYY-MM-DD HH:MM:SS".
    /// </summary>
    [XmlIgnore]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of the spot as a string.
    /// </summary>
    [XmlElement("timestamp")]
    [JsonIgnore]
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

    /// <summary>
    ///     Gets or sets the action for the spot (e.g., add, delete).
    /// </summary>
    [XmlElement("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the mode of operation (e.g., CW, SSB).
    /// </summary>
    [XmlElement("mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the comment associated with the spot.
    /// </summary>
    [XmlElement("comment")]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the status of the spot (e.g., dupe, single mult).
    /// </summary>
    [XmlElement("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the list of statuses associated with the spot.
    /// </summary>
    [XmlElement("statuslist")]
    public string StatusList { get; set; } = string.Empty;
}