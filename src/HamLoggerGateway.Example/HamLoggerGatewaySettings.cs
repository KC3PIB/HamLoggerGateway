namespace HamLoggerGateway.Example;

/// <summary>
///     Represents the configuration settings for the HamLoggerGateway.
/// </summary>
public class HamLoggerGatewaySettings
{
    /// <summary>
    ///     Gets or sets the settings for the N1MM UDP server.
    /// </summary>
    /// <remarks>
    ///     This property can be null if the UDP server is not used.
    /// </remarks>
    public ServerSettings? N1MMUdpServer { get; set; }

    /// <summary>
    ///     Gets or sets the settings for the N1MM TCP server.
    /// </summary>
    /// <remarks>
    ///     This property can be null if the TCP server is not used.
    /// </remarks>
    public ServerSettings? N1MMTcpServer { get; set; }
}