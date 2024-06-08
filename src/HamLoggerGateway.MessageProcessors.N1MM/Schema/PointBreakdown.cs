using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the breakdown of points by band and mode.
/// </summary>
public class PointBreakdown
{
    /// <summary>
    ///     Gets or sets the band of the points.
    /// </summary>
    [XmlAttribute("band")]
    public string Band { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the mode of the points.
    /// </summary>
    [XmlAttribute("mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the number of points.
    /// </summary>
    [XmlText]
    public int PointCount { get; set; }
}