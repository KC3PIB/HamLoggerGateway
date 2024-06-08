using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the dynamic results sent via UDP messages for score reporting.
///     This includes details such as the contest, operator callsign, class, location, and score breakdown.
/// </summary>
[XmlRoot("dynamicresults")]
public class DynamicResults
{
    /// <summary>
    ///     Gets or sets the contest name.
    /// </summary>
    [XmlElement("contest")]
    public string Contest { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the operator callsign.
    /// </summary>
    [XmlElement("call")]
    public string Call { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the operators involved.
    /// </summary>
    [XmlElement("ops")]
    public string Ops { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the class details of the contest entry.
    /// </summary>
    [XmlElement("class")]
    public ContestClass Class { get; set; } = new();

    /// <summary>
    ///     Gets or sets the club name.
    /// </summary>
    [XmlElement("club")]
    public string Club { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the location details of the station.
    /// </summary>
    [XmlElement("qth")]
    public Location Location { get; set; } = new();

    /// <summary>
    ///     Gets or sets the breakdown of QSOs and points.
    /// </summary>
    [XmlElement("breakdown")]
    public Breakdown Breakdown { get; set; } = new();

    /// <summary>
    ///     Gets or sets the total score.
    /// </summary>
    [XmlElement("score")]
    public int Score { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of the score report. The format is expected to be "YYYY-MM-DD HH:MM:SS".
    /// </summary>
    [XmlIgnore]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of the score report as a string.
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
}