using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace StubLogger.UnitTests;

[SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging")]
public class StubLoggerTests
{
    private readonly Faker _faker = new();
    private readonly StubLogger<StubLoggerTests> _sut = new();

    [Fact]
    public void AssertLogEvent_WhenOneWarningEventIsCreated_ThenItCanBeAsserted()
    {
        // arrange & act
        _sut.LogWarning("This is a warning message with value: {Value}", _faker.Random.Int());

        // assert
        _sut.AssertLogEvent(e =>
            e.LogLevel == LogLevel.Warning && e.Message.Contains("This is a warning message with value:"));
    }

    [Fact]
    public void AssertLogEvent_WhenNoMatchingEventIsCreated_ThenItFails()
    {
        // arrange
        _sut.LogInformation("This is an info message with value: {Value}", _faker.Random.Int());

        // act
        var action = () => _sut.AssertLogEvent(e =>
            e.LogLevel == LogLevel.Warning && e.Message.Contains("This is a warning message with value:"));

        // assert
        action.Should()
            .Throw<NotNullException>();
    }

    [Fact]
    public void AssertLogEvent_WhenExceptionIsLogged_ThenItCanBeAsserted()
    {
        // arrange
        var exception = new InvalidOperationException("Invalid operation occurred");
        _sut.LogError(exception, "An error occurred while processing");

        // act & assert
        _sut.AssertLogEvent(e =>
            e.LogLevel == LogLevel.Error &&
            e.Message.Contains("An error occurred while processing") &&
            e.Exception == exception);
    }

    [Fact]
    public void
        AssertLogEvent_WhenExceptionIsLogged_ThenItCanBeMatchedWithoutTheException_AndTheExceptionCanBeAssertedIndependently()
    {
        // arrange
        var exception = new InvalidOperationException("Invalid operation occurred");
        _sut.LogError(exception, "An error occurred while processing");

        // act & assert
        _sut.AssertLogEvent(e =>
            e.LogLevel == LogLevel.Error &&
            e.Message.Contains("An error occurred while processing"), e => e.Exception.Should()
            .BeOfType<InvalidOperationException>());
    }

    [Fact]
    public void
        AssertLogEvent_WhenExceptionIsLogged_ThenItCanBeMatchedButFailOnTheExceptionAssertion()
    {
        // arrange
        var exception = new InvalidOperationException("Invalid operation occurred");
        _sut.LogError(exception, "An error occurred while processing");

        // act
        var action = () => _sut.AssertLogEvent(e =>
            e.LogLevel == LogLevel.Error &&
            e.Message.Contains("An error occurred while processing"), e => e.Exception.Should()
            .BeOfType<ArgumentNullException>());

        // assert
        action.Should()
            .Throw<XunitException>();
    }

    [Fact]
    public void AssertLogCount_WhenMultipleLogEventsAreCreated_ThenItCanBeAsserted()
    {
        // arrange
        _sut.LogInformation("Info message 1");
        _sut.LogInformation("Info message 2");
        _sut.LogWarning("Warning message 1");

        // act & assert
        _sut.AssertLogCount(e => e.LogLevel == LogLevel.Information, 2);
        _sut.AssertLogCount(e => e.LogLevel == LogLevel.Warning, 1);
    }

    [Fact]
    public void AssertLogCount_WhenCountsAreIncorrect_ThenItFails()
    {
        // arrange
        _sut.LogInformation("Info message 1");
        _sut.LogWarning("Warning message 1");

        // act
        var actionInfo = () => _sut.AssertLogCount(e => e.LogLevel == LogLevel.Information, 2);
        var actionWarning = () => _sut.AssertLogCount(e => e.LogLevel == LogLevel.Warning, 2);

        // assert
        actionInfo.Should()
            .Throw<XunitException>();

        actionWarning.Should()
            .Throw<XunitException>();
    }

    [Theory]
    [MemberData(nameof(LogLevelTrace))]
    public void IsEnabled_WhenCalledConfiguredToTrace_ThenReturnsTrueForTraceOrLower(LogLevel level, bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Trace, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelDebug))]
    public void IsEnabled_WhenCalledConfiguredToDebug_ThenReturnsTrueForTraceOrLower(LogLevel level, bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Debug, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelInformation))]
    public void IsEnabled_WhenCalledConfiguredToInformation_ThenReturnsTrueForInformationOrLower(LogLevel level,
        bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Information, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelWarning))]
    public void IsEnabled_WhenCalledConfiguredToWarning_ThenReturnsTrueForInformationOrLower(LogLevel level,
        bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Warning, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelError))]
    public void IsEnabled_WhenCalledConfiguredToError_ThenReturnsTrueForInformationOrLower(LogLevel level,
        bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Error, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelCritical))]
    public void IsEnabled_WhenCalledConfiguredToCritical_ThenReturnsTrueForInformationOrLower(LogLevel level,
        bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.Critical, level, expected);
    }

    [Theory]
    [MemberData(nameof(LogLevelNone))]
    public void IsEnabled_WhenCalledConfiguredToNone_ThenReturnsTrueForInformationOrLower(LogLevel level, bool expected)
    {
        AssertIsEnabledLogLevels(LogLevel.None, level, expected);
    }

    private void AssertIsEnabledLogLevels(LogLevel configureLevel, LogLevel testLevel, bool expected)
    {
        // arrange
        _sut.LoggerLogLevel = configureLevel;

        // act
        var isEnabled = _sut.IsEnabled(testLevel);

        // assert
        isEnabled.Should()
            .Be(expected, $"IsEnabled should be {expected} for {testLevel}");
    }

    public static TheoryData<LogLevel, bool> LogLevelTrace => LogLevelExpectations(LogLevel.Trace);
    public static TheoryData<LogLevel, bool> LogLevelDebug => LogLevelExpectations(LogLevel.Debug);
    public static TheoryData<LogLevel, bool> LogLevelInformation => LogLevelExpectations(LogLevel.Information);
    public static TheoryData<LogLevel, bool> LogLevelWarning => LogLevelExpectations(LogLevel.Warning);
    public static TheoryData<LogLevel, bool> LogLevelError => LogLevelExpectations(LogLevel.Error);
    public static TheoryData<LogLevel, bool> LogLevelCritical => LogLevelExpectations(LogLevel.Critical);
    public static TheoryData<LogLevel, bool> LogLevelNone => LogLevelExpectations(LogLevel.None);

    private static TheoryData<LogLevel, bool> LogLevelExpectations(LogLevel expectation)
    {
        var data = new TheoryData<LogLevel, bool>();
        foreach (var level in Enum.GetValues<LogLevel>())
        {
            var expected = (int) level <= (int) expectation;
            data.Add(level, expected);
        }

        return data;
    }
}