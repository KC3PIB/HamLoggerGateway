using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents lookup information intended for external callsign lookup,
///     including basic contest and station details. This packet is sent to
///     assist third-party applications with additional context for ongoing
///     contest contacts.
/// </summary>
[XmlRoot("lookupinfo")]
public class LookupInfo : ContactInfo
{
}