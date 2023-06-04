using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class ResultTests {
  [Fact]
  public void ReturnsSuccessWhenValueIsProvided() {
    const string value = "test value";
    Result<string> result = Result<string>.Ok(value);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void ReturnsFailureWhenErrorIsProvided() {
    Exception error = new("test error");
    Result<string> result = Result<string>.Fail(error);

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(error);
  }

  [Fact]
  public void ThrowsExceptionWhenAccessingValueOnFailure() {
    Exception error = new("test error");
    Result<string> result = Result<string>.Fail(error);
    Action act = () => _ = result.Value;

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void ThrowsExceptionWhenAccessingErrorOnSuccess() {
    const string value = "test value";
    Result<string> result = Result<string>.Ok(value);
    Action act = () => _ = result.Error;

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void ThenReturnsSuccessWhenBothResultsAreSuccessful() {
    Result<string> initialResult = Result<string>.Ok("test value");
    Result<int> nextResult = Result<int>.Ok(123);
    Result<int> TransformToInt(string _) => nextResult;

    Result<int> finalResult = initialResult.Then(TransformToInt);

    finalResult.IsSuccess.Should().BeTrue();
    finalResult.Value.Should().Be(nextResult.Value);
  }

  [Fact]
  public void ThenReturnsFailureWhenInitialResultIsFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("initial error"));
    Result<int> nextResult = Result<int>.Ok(123);
    Result<int> TransformToInt(string _) => nextResult;

    Result<int> finalResult = initialResult.Then((Func<string, Result<int>>)TransformToInt);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialResult.Error);
  }
}