using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the location details of the station.
/// </summary>
public class Location
{
    /// <summary>
    ///     Gets or sets the DXCC country.
    /// </summary>
    [XmlElement("dxcccountry")]
    public string DxccCountry { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the CQ zone.
    /// </summary>
    [XmlElement("cqzone")]
    public int CqZone { get; set; }

    /// <summary>
    ///     Gets or sets the IARU zone.
    /// </summary>
    [XmlElement("iaruzone")]
    public int IaruZone { get; set; }

    /// <summary>
    ///     Gets or sets the ARRL section.
    /// </summary>
    [XmlElement("arrlsection")]
    public string ArrlSection { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the state or province.
    /// </summary>
    [XmlElement("stprvoth")]
    public string StateProvinceOther { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the 6-character grid square.
    /// </summary>
    [XmlElement("grid6")]
    public string Grid6 { get; set; } = string.Empty;
}