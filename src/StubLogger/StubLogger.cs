using Microsoft.Extensions.Logging;
using Xunit;

namespace StubLogger;

/// <summary>
/// A stub logger that tracks log events for unit testing purposes.
/// </summary>
/// <typeparam name="T">The type for which the logger is created.</typeparam>
public class StubLogger<T> : ILogger<T>
{
    private readonly List<TrackedLogEvent> _tracker = [];

    /// <inheritdoc/>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var tracked = new TrackedLogEvent(logLevel, formatter(state, exception), exception);
        _tracker.Add(tracked);
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Asserts that a log event matching the given predicate exists.
    /// </summary>
    /// <param name="predicate">The predicate to match log events.</param>
    /// <param name="assertion">An optional action to perform additional assertions on the matched log event.</param>
    public void AssertLogEvent(Func<TrackedLogEvent, bool> predicate, Action<TrackedLogEvent>? assertion = null)
    {
        var tracked = _tracker.FirstOrDefault(predicate);
        Assert.NotNull(tracked);

        assertion?.Invoke(tracked);
    }

    /// <summary>
    /// Asserts that the count of log events matching the given predicate equals the expected count.
    /// </summary>
    /// <param name="predicate">The predicate to match log events.</param>
    /// <param name="expectedCount">The expected count of matching log events.</param>
    public void AssertLogCount(Func<TrackedLogEvent, bool> predicate, int expectedCount)
    {
        var count = _tracker.Count(predicate);
        Assert.Equal(expectedCount, count);
    }
}

/// <summary>
/// Represents a tracked log event with its level, message, and optional exception.
/// </summary>
/// <param name="LogLevel">The log level of the event.</param>
/// <param name="Message">The log message.</param>
/// <param name="Exception">The optional exception associated with the log event.</param>
public record TrackedLogEvent(LogLevel LogLevel, string Message, Exception? Exception);