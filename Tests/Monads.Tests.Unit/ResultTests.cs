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

  [Fact]
  public void ThenReturnsFailureWhenNextResultIsFailure() {
    Result<string> initialResult = Result<string>.Ok("test value");
    Result<int> nextResult = Result<int>.Fail(new Exception("next error"));
    Result<int> TransformToIntError(string _) => nextResult;

    Result<int> finalResult = initialResult.Then(TransformToIntError);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(nextResult.Error);
  }

  [Fact]
  public void MapTransformsValueOnSuccess() {
    Result<string> initialResult = Result<string>.Ok("one");
    const int result = 1;
    int MapFunc(string str) => result;

    Result<int> mappedResult = initialResult.Map(MapFunc);

    mappedResult.IsSuccess.Should().BeTrue();
    mappedResult.Value.Should().Be(result);
  }

  [Fact]
  public void MapPreservesErrorOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    int MapFunc(string str) => 1;

    Result<int> mappedResult = initialResult.Map(MapFunc);

    mappedResult.IsSuccess.Should().BeFalse();
    mappedResult.Error.Should().Be(initialResult.Error);
  }

  [Fact]
  public void MapDoesNotInvokeFunctionOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    bool wasInvoked = false;

    int MapFunc(string str) {
      wasInvoked = true;
      return 1;
    }

    _ = initialResult.Map(MapFunc);
    wasInvoked.Should().BeFalse();
  }

  [Fact]
  public void MatchInvokesSuccessFunctionOnSuccess() {
    Result<string> initialResult = Result<string>.Ok("test value");
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    int OnSuccess(string value) {
      wasSuccessInvoked = true;
      return value.Length;
    }

    int OnFailure(Exception error) {
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = initialResult.Match(OnSuccess, OnFailure);

    wasSuccessInvoked.Should().BeTrue();
    wasFailureInvoked.Should().BeFalse();
    matchResult.Should().Be(initialResult.Value.Length);
  }

  [Fact]
  public void MatchInvokesFailureFunctionOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    int OnSuccess(string value) {
      wasSuccessInvoked = true;
      return value.Length;
    }

    int OnFailure(Exception error) {
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = initialResult.Match(OnSuccess, OnFailure);

    wasSuccessInvoked.Should().BeFalse();
    wasFailureInvoked.Should().BeTrue();
    matchResult.Should().Be(initialResult.Error.Message.Length);
  }
}