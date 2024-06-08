using System.Collections.Concurrent;
using System.Diagnostics;

namespace HamLoggerGateway.Utils;

/// <summary>
///     Provides a mechanism for limiting the rate of requests over a specified time window.
/// </summary>
internal class RateLimiter
{
    /// <summary>
    ///     Maximum number of requests allowed in the time window.
    /// </summary>
    private readonly int _maxRequests;

    /// <summary>
    ///     Queue that stores timestamps of the requests.
    /// </summary>
    private readonly ConcurrentQueue<long> _requestTimestamps;

    /// <summary>
    ///     Stopwatch used for calculating elapsed time for rate limiting.
    /// </summary>
    private readonly Stopwatch _stopwatch;

    /// <summary>
    ///     Size of the time window for rate limiting, measured in stopwatch ticks.
    /// </summary>
    private readonly long _windowSizeTicks;

    /// <summary>
    ///     Stopwatch ticks recorded at the last cleanup operation.
    /// </summary>
    private long _lastCleanupTick;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RateLimiter" /> class.
    /// </summary>
    /// <param name="maxRequests">The maximum number of requests that are allowed in the given window.</param>
    /// <param name="windowSize">The duration of the window for which the rate limit applies.</param>
    public RateLimiter(int maxRequests, TimeSpan windowSize)
    {
        _maxRequests = maxRequests;
        _windowSizeTicks = windowSize.Ticks;
        _requestTimestamps = new ConcurrentQueue<long>();
        _stopwatch = Stopwatch.StartNew();
        _lastCleanupTick = _stopwatch.ElapsedTicks;
    }

    /// <summary>
    ///     Gets the last UTC time when the request was allowed.
    /// </summary>
    public DateTime LastAccessed { get; private set; }

    /// <summary>
    ///     Determines whether a new request can be allowed at the current time.
    /// </summary>
    /// <returns>true if the request is allowed; otherwise, false.</returns>
    public bool AllowRequest()
    {
        LastAccessed = DateTime.UtcNow;
        var nowTicks = _stopwatch.ElapsedTicks;

        CleanupOldRequests(nowTicks);

        if (_requestTimestamps.Count >= _maxRequests) return false;

        _requestTimestamps.Enqueue(nowTicks);
        return true;
    }

    /// <summary>
    ///     Cleans up old requests that fall outside of the time window.
    /// </summary>
    /// <param name="currentTicks">The current tick count from the stopwatch.</param>
    private void CleanupOldRequests(long currentTicks)
    {
        // Only clean up if sufficient time has passed since the last cleanup
        if (currentTicks - _lastCleanupTick > _windowSizeTicks)
        {
            var cutoff = currentTicks - _windowSizeTicks;
            while (_requestTimestamps.TryPeek(out var oldTick) &&
                   oldTick < cutoff)
                _requestTimestamps.TryDequeue(out _);

            _lastCleanupTick = currentTicks; // Update the last cleanup time
        }
    }
}