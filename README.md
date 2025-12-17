# StubLogger

[![NuGet](https://img.shields.io/nuget/v/StubLogger.svg)](https://www.nuget.org/packages/StubLogger/)

A lightweight stub logger for unit testing with xUnit that tracks and asserts
log events in .NET applications.

## Overview

StubLogger provides a simple and intuitive way to verify logging behaviour
in your unit tests. It implements `ILogger<T>` and tracks all log events,
allowing you to assert that specific log messages were written with the
correct log levels and exceptions.

## Installation

Install the StubLogger NuGet package:

```bash
dotnet add package StubLogger
```

Or via the Package Manager Console:

```powershell
Install-Package StubLogger
```

## Usage

### Basic Usage

Create a `StubLogger<T>` instance in your test and inject it into the class
under test:

```csharp
using Microsoft.Extensions.Logging;
using StubLogger;
using Xunit;

public class MyServiceTests
{
    private readonly StubLogger<MyService> _logger = new();
    private readonly MyService _sut;

    public MyServiceTests()
    {
        _sut = new MyService(_logger);
    }

    [Fact]
    public void ProcessData_WhenCalled_LogsInformation()
    {
        // arrange & act
        _sut.ProcessData();

        // assert
        _logger.AssertLogEvent(e => 
            e.LogLevel == LogLevel.Information && 
            e.Message.Contains("Processing data"));
    }
}
```

### Asserting Log Events

Use `AssertLogEvent` to verify that a specific log event was written:

```csharp
[Fact]
public void Method_WhenWarningCondition_LogsWarning()
{
    // arrange & act
    _sut.MethodWithWarning();

    // assert
    _logger.AssertLogEvent(e => 
        e.LogLevel == LogLevel.Warning && 
        e.Message.Contains("Expected warning message"));
}
```

### Asserting Log Event Count

Use `AssertLogCount` to verify the number of log events that match a predicate:

```csharp
[Fact]
public void Method_WhenCalledMultipleTimes_LogsCorrectCount()
{
    // arrange & act
    _sut.ProcessItem("Item1");
    _sut.ProcessItem("Item2");
    _sut.ProcessItem("Item3");

    // assert
    _logger.AssertLogCount(e => 
        e.LogLevel == LogLevel.Information, 3);
}
```

### Asserting Exceptions

Verify that exceptions are logged correctly:

```csharp
[Fact]
public void Method_WhenExceptionOccurs_LogsError()
{
    // arrange
    var exception = new InvalidOperationException("Invalid operation");

    // act
    _sut.HandleError(exception);

    // assert
    _logger.AssertLogEvent(e => 
        e.LogLevel == LogLevel.Error && 
        e.Message.Contains("An error occurred") && 
        e.Exception == exception);
}
```

### Advanced Exception Assertions

You can use a separate assertion action to verify exception details:

```csharp
[Fact]
public void Method_WhenExceptionOccurs_LogsErrorWithCorrectExceptionType()
{
    // arrange
    var exception = new InvalidOperationException("Invalid operation");

    // act
    _sut.HandleError(exception);

    // assert
    _logger.AssertLogEvent(
        e => e.LogLevel == LogLevel.Error && 
             e.Message.Contains("An error occurred"),
        e => e.Exception.Should().BeOfType<InvalidOperationException>());
}
```

## API Reference

### `StubLogger<T>`

Implements `ILogger<T>` and provides assertion methods for unit testing.

#### Methods

- **`AssertLogEvent(Func<TrackedLogEvent, bool> predicate,
  Action<TrackedLogEvent>? assertion = null)`**

  Asserts that a log event matching the given predicate exists. Optionally
  performs additional assertions on the matched log event.
  
  - `predicate`: A function to match log events
  - `assertion`: An optional action to perform additional assertions

- **`AssertLogCount(Func<TrackedLogEvent, bool> predicate,
  int expectedCount)`**

  Asserts that the count of log events matching the given predicate equals
  the expected count.
  
  - `predicate`: A function to match log events
  - `expectedCount`: The expected number of matching log events

### `TrackedLogEvent`

Represents a tracked log event with the following properties:

- **`LogLevel`**: The log level of the event (Information, Warning, Error,
  etc.)
- **`Message`**: The formatted log message
- **`Exception`**: The optional exception associated with the log event
  (null if no exception)

## Requirements

- .NET 8.0, 9.0, or 10.0
- xUnit 2.9.3 or higher
- Microsoft.Extensions.Logging.Abstractions 10.0.0 or higher

## License

This project is licensed under
[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).
