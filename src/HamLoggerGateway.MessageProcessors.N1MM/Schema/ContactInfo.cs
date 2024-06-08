using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents contact information sent via UDP messages, detailing each contact (QSO) logged.
///     This includes timestamps, callsigns, frequencies, modes, and various contest-specific
///     exchange information.
/// </summary>
[XmlRoot("contactinfo")]
public class ContactInfo
{
    /// <summary>
    ///     Short name of the logger sending the packet. Examples: “N1MM”, “DXLab”, “Logger32”, “UcxLog”, “Log4OM”
    /// </summary>
    [XmlElement("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///     The contest name.
    /// </summary>
    [XmlElement("contestname")]
    public string ContestName { get; set; } = string.Empty;

    /// <summary>
    ///     The contest number. This number is unique to the contest within the database.
    /// </summary>
    [XmlElement("contestnr")]
    public int ContestNr { get; set; }

    /// <summary>
    ///     Timestamp of the contact.
    /// </summary>
    [XmlIgnore]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///     The timestamp of the contact as a string. The format is expected to be "YYYY-MM-DD HH:MM:SS".
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
    ///     The operator's callsign.
    /// </summary>
    [XmlElement("mycall")]
    public string MyCall { get; set; } = string.Empty;

    /// <summary>
    ///     The band of operation. This is a double representation to accommodate different
    ///     localizations, e.g., "3.5" or "3,5" for 80 meters depending on the user's Windows settings.
    /// </summary>
    [XmlElement("band")]
    public double Band { get; set; }

    /// <summary>
    ///     The receive frequency in units of 10 Hz.
    /// </summary>
    [XmlElement("rxfreq")]
    public int RxFreq { get; set; }

    /// <summary>
    ///     The transmit frequency in units of 10 Hz.
    /// </summary>
    [XmlElement("txfreq")]
    public int TxFreq { get; set; }

    /// <summary>
    ///     The operator's name.
    /// </summary>
    [XmlElement("operator")]
    public string Operator { get; set; } = string.Empty;

    /// <summary>
    ///     The mode of operation (e.g., CW, SSB).
    /// </summary>
    [XmlElement("mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     The callsign of the contacted station.
    /// </summary>
    [XmlElement("call")]
    public string Call { get; set; } = string.Empty;

    /// <summary>
    ///     The country prefix of the contacted station.
    /// </summary>
    [XmlElement("countryprefix")]
    public string CountryPrefix { get; set; } = string.Empty;

    /// <summary>
    ///     The WPX prefix of the contacted station.
    /// </summary>
    [XmlElement("wpxprefix")]
    public string WpxPrefix { get; set; } = string.Empty;

    /// <summary>
    ///     The station prefix of the contacted station.
    /// </summary>
    [XmlElement("stationprefix")]
    public string StationPrefix { get; set; } = string.Empty;

    /// <summary>
    ///     The continent of the contacted station.
    /// </summary>
    [XmlElement("continent")]
    public string Continent { get; set; } = string.Empty;

    /// <summary>
    ///     The sent exchange information.
    /// </summary>
    [XmlElement("snt")]
    public string Snt { get; set; } = string.Empty;

    /// <summary>
    ///     The sent serial number.
    /// </summary>
    [XmlElement("sntnr")]
    public int SntNr { get; set; }

    /// <summary>
    ///     The received exchange information.
    /// </summary>
    [XmlElement("rcv")]
    public string Rcv { get; set; } = string.Empty;

    /// <summary>
    ///     The received serial number.
    /// </summary>
    [XmlElement("rcvnr")]
    public int RcvNr { get; set; }

    /// <summary>
    ///     The grid square of the contacted station.
    /// </summary>
    [XmlElement("gridsquare")]
    public string GridSquare { get; set; } = string.Empty;

    /// <summary>
    ///     The additional exchange information.
    /// </summary>
    [XmlElement("exchangel")]
    public string ExchangeL { get; set; } = string.Empty;

    /// <summary>
    ///     The section of the contacted station.
    /// </summary>
    [XmlElement("section")]
    public string Section { get; set; } = string.Empty;

    /// <summary>
    ///     The comment for the contact.
    /// </summary>
    [XmlElement("comment")]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    ///     The QTH (location) of the contacted station.
    /// </summary>
    [XmlElement("qth")]
    public string Qth { get; set; } = string.Empty;

    /// <summary>
    ///     The name of the contacted station operator.
    /// </summary>
    [XmlElement("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     The power level of the contacted station.
    /// </summary>
    [XmlElement("power")]
    public string Power { get; set; } = string.Empty;

    /// <summary>
    ///     Miscellaneous text for the contact.
    /// </summary>
    [XmlElement("misctext")]
    public string MiscText { get; set; } = string.Empty;

    /// <summary>
    ///     The zone of the contacted station.
    /// </summary>
    [XmlElement("zone")]
    public int Zone { get; set; }

    /// <summary>
    /// </summary>
    [XmlElement("prec")]
    public string Prec { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [XmlElement("ck")]
    public int Ck { get; set; }

    /// <summary>
    ///     Whether the contacted station is a multiplier for the local station.
    /// </summary>
    [XmlElement("ismultiplierl")]
    public string IsMultiplierL { get; set; } = string.Empty;

    /// <summary>
    ///     A secondary multiplier value for the contact.
    /// </summary>
    [XmlElement("ismultiplier2")]
    public int IsMultiplier2 { get; set; }

    /// <summary>
    ///     A tertiary multiplier value for the contact.
    /// </summary>
    [XmlElement("ismultiplier3")]
    public int IsMultiplier3 { get; set; }

    /// <summary>
    ///     The points associated with the contact.
    /// </summary>
    [XmlElement("points")]
    public string Points { get; set; } = string.Empty;

    /// <summary>
    ///     The radio number for the contact.
    /// </summary>
    [XmlElement("radionr")]
    public string RadioNr { get; set; } = string.Empty;

    /// <summary>
    ///     The run1/run2 status for the contact.
    /// </summary>
    [XmlElement("run1run2")]
    public string Run1Run2 { get; set; } = string.Empty;

    /// <summary>
    ///     The rover location for the contact.
    /// </summary>
    [XmlElement("RoverLocation")]
    public string RoverLocation { get; set; } = string.Empty;

    /// <summary>
    ///     The radio is interfaced for the contact.
    /// </summary>
    [XmlElement("RadioInterfaced")]
    public string RadioInterfaced { get; set; } = string.Empty;

    /// <summary>
    ///     The networked computer number for the contact.
    /// </summary>
    [XmlElement("NetworkedCompNr")]
    public int NetworkedCompNr { get; set; }

    /// <summary>
    ///     Whether the contact is original.
    /// </summary>
    [XmlIgnore]
    public bool IsOriginal { get; set; }

    /// <summary>
    ///     Whether the contact is original as a string.
    /// </summary>
    [XmlElement("IsOriginal")]
    [JsonIgnore]
    public string IsOriginalString
    {
        get => IsOriginal.ToString();
        set
        {
            if (!bool.TryParse(value, out var result))
                throw new InvalidOperationException($"Invalid boolean value '{value}' for IsOriginal.");
            IsOriginal = result;
        }
    }

    /// <summary>
    ///     The NetBIOS name for the contact.
    /// </summary>
    [XmlElement("NetBiosName")]
    public string NetBiosName { get; set; } = string.Empty;

    /// <summary>
    ///     Whether the contact is a run QSO.
    /// </summary>
    [XmlElement("IsRunQSO")]
    public int IsRunQso { get; set; }

    /// <summary>
    ///     The station name for the contact.
    /// </summary>
    [XmlElement("StationName")]
    public string StationName { get; set; } = string.Empty;

    /// <summary>
    ///     The ID for the contact.
    /// </summary>
    [XmlElement("ID")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Whether the contact is claimed as a QSO.
    /// </summary>
    [XmlElement("IsClaimedQso")]
    public int IsClaimedQso { get; set; }
}