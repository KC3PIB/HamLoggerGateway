using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents N1MM application information sent via UDP messages, providing details
///     about the station and the current contest. This information includes the
///     application name, database name, contest number and name, and the station name.
/// </summary>
[XmlRoot("AppInfo")]
public class AppInfo
{
    /// <summary>
    ///     The application name
    /// </summary>
    [XmlElement("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///     The database file name in use
    /// </summary>
    [XmlElement("dbname")]
    public string DbName { get; set; } = string.Empty;

    /// <summary>
    ///     A unique number assigned to this contest in this database
    ///     <remarks>Do not expect the same contestnr to be assigned to another computer running the same contest name.</remarks>
    /// </summary>
    [XmlElement("contestnr")]
    public int ContestNr { get; set; }

    /// <summary>
    ///     The name of the contest
    /// </summary>
    [XmlElement("contestname")]
    public string ContestName { get; set; } = string.Empty;

    /// <summary>
    ///     The name of the station
    /// </summary>
    [XmlElement("StationName")]
    public string StationName { get; set; } = string.Empty;
}