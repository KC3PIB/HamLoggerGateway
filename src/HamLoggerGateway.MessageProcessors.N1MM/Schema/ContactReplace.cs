using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents a contact replacement information packet, mirroring the structure
///     of the ContactInfo packet but intended for replacing an existing contact record.
/// </summary>
[XmlRoot("contactreplace")]
public class ContactReplace : ContactInfo
{
}