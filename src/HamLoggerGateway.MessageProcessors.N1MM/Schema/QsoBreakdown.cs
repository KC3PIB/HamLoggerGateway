using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the breakdown of QSOs by band and mode.
/// </summary>
public class QsoBreakdown
{
    /// <summary>
    ///     Gets or sets the band of the QSOs.
    /// </summary>
    [XmlAttribute("band")]
    public string Band { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the mode of the QSOs.
    /// </summary>
    [XmlAttribute("mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the number of QSOs.
    /// </summary>
    [XmlText]
    public int QsoCount { get; set; }
}