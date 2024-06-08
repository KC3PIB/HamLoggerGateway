using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents the breakdown of QSOs and points.
/// </summary>
public class Breakdown
{
    /// <summary>
    ///     Gets or sets the list of QSOs by band and mode.
    /// </summary>
    [XmlElement("qso")]
    public List<QsoBreakdown> QsoList { get; set; } = new();

    /// <summary>
    ///     Gets or sets the list of points by band and mode.
    /// </summary>
    [XmlElement("point")]
    public List<PointBreakdown> PointList { get; set; } = new();
}