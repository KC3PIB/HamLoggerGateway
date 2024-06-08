using System.Net.Sockets;

namespace HamLoggerGateway;

/// <summary>
///     Provides configuration settings for the server.
/// </summary>
public class ServerSettings
{
    /// <summary>
    ///     Gets or sets the IP address the server will bind to.
    /// </summary>
    /// <value>
    ///     The IP address as a string. Defaults to "::1" (IPv6 localhost).
    /// </value>
    public string Address { get; set; } = "::1";

    /// <summary>
    ///     Gets or sets the port number on which the server will listen.
    /// </summary>
    /// <value>
    ///     The port number. Valid port numbers are between 1 and 65535.
    /// </value>
    public int Port { get; set; }

    /// <summary>
    ///     Gets or sets the size of the buffer used for incoming messages.
    /// </summary>
    /// <value>
    ///     The size of the buffer in bytes. If not specified, defaults are used based on the protocol.
    /// </value>
    public int? BufferSize { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="SocketOptionName.ReuseAddress" /> option is enabled on the
    ///     server socket.
    /// </summary>
    /// <value>
    ///     <c>true</c> if <see cref="SocketOptionName.ReuseAddress" /> is enabled; otherwise, <c>false</c>.
    ///     This option facilitates the reuse of an address for a socket and is enabled by default.
    /// </value>
    public bool EnableReuseAddress { get; set; } = true;

    /// <summary>
    ///     Gets or sets the maximum number of requests per minute allowed from a single IP address for UDP servers.
    /// </summary>
    /// <value>
    ///     The rate limit as the number of requests per minute. Defaults to 60.
    /// </value>
    public int RequestsPerMinutePerIp { get; set; } = 60;
}