using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the class details of the contest entry.
/// </summary>
public class ContestClass
{
    /// <summary>
    ///     Gets or sets the power level (e.g., HIGH, LOW, QRP).
    /// </summary>
    [XmlAttribute("power")]
    public string Power { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets whether the entry is assisted.
    /// </summary>
    [XmlAttribute("assisted")]
    public string Assisted { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the number of transmitters (e.g., UNLIMITED, ONE).
    /// </summary>
    [XmlAttribute("transmitter")]
    public string Transmitter { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the operating category (e.g., MULTI-OP, SINGLE-OP).
    /// </summary>
    [XmlAttribute("ops")]
    public string Ops { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the bands used (e.g., ALL, SINGLE).
    /// </summary>
    [XmlAttribute("bands")]
    public string Bands { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the mode (e.g., MIXED, CW, SSB).
    /// </summary>
    [XmlAttribute("mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the overlay category (e.g., N/A, CLASSIC).
    /// </summary>
    [XmlAttribute("overlay")]
    public string Overlay { get; set; } = string.Empty;
}