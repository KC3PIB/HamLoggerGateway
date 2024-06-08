using System.Net;

namespace HamLoggerGateway.Utils;

/// <summary>
///     Provides functionality to manage and check IP addresses against a blacklist of IP ranges.
/// </summary>
public static class HostBlackList
{
    /// <summary>
    ///     Dictionary to store blacklisted IP ranges grouped by service name.
    /// </summary>
    private static readonly Dictionary<string, List<IPNetwork>> BlacklistedRanges = new();

    /// <summary>
    ///     Initializes static members of the <see cref="HostBlackList" /> class.
    ///     Populates the blacklist with predefined IP ranges associated with specific services.
    /// </summary>
    static HostBlackList()
    {
        // Add blacklisted IPs for "internet-measurement.com"
        BlacklistedRanges.Add("internet-measurement.com", new List<IPNetwork>
        {
            IPNetwork.Parse("87.236.176.0/24"),
            IPNetwork.Parse("193.163.125.0/24"),
            IPNetwork.Parse("68.183.53.77/32"),
            IPNetwork.Parse("104.248.203.191/32"),
            IPNetwork.Parse("104.248.204.195/32"),
            IPNetwork.Parse("142.93.191.98/32"),
            IPNetwork.Parse("157.245.216.203/32"),
            IPNetwork.Parse("165.22.39.64/32"),
            IPNetwork.Parse("167.99.209.184/32"),
            IPNetwork.Parse("188.166.26.88/32"),
            IPNetwork.Parse("206.189.7.178/32"),
            IPNetwork.Parse("209.97.152.248/32"),
            IPNetwork.Parse("2a06:4880::/32"),
            IPNetwork.Parse("2604:a880:800:10::c4b:f000/124"),
            IPNetwork.Parse("2604:a880:800:10::c51:a000/124"),
            IPNetwork.Parse("2604:a880:800:10::c52:d000/124"),
            IPNetwork.Parse("2604:a880:800:10::c55:5000/124"),
            IPNetwork.Parse("2604:a880:800:10::c56:b000/124"),
            IPNetwork.Parse("2a03:b0c0:2:d0::153e:a000/124"),
            IPNetwork.Parse("2a03:b0c0:2:d0::1576:8000/124"),
            IPNetwork.Parse("2a03:b0c0:2:d0::1577:7000/124"),
            IPNetwork.Parse("2a03:b0c0:2:d0::1579:e000/124"),
            IPNetwork.Parse("2a03:b0c0:2:d0::157c:a000/124")
        });
    }

    /// <summary>
    ///     Adds a new set of IP ranges to the blacklist under a specified label.
    /// </summary>
    /// <param name="label">The label associated with the IP ranges.</param>
    /// <param name="networks">The list of IP ranges to add to the blacklist.</param>
    public static void Add(string label, List<IPNetwork> networks)
    {
        BlacklistedRanges.Add(label, networks);
    }

    /// <summary>
    ///     Determines whether the specified IP endpoint is blacklisted.
    /// </summary>
    /// <param name="endPoint">The IP endpoint to check.</param>
    /// <param name="service">Outputs the service name if the IP endpoint is blacklisted; otherwise, null.</param>
    /// <returns>true if the IP endpoint is blacklisted; otherwise, false.</returns>
    public static bool IsBlacklisted(IPEndPoint endPoint, out string? service)
    {
        var address = !endPoint.Address.IsIPv4MappedToIPv6 ? endPoint.Address : endPoint.Address.MapToIPv4();

        foreach (var entry in BlacklistedRanges)
            if (entry.Value.Any(range => range.Contains(address)))
            {
                service = entry.Key;
                return true;
            }

        service = null;
        return false;
    }
}