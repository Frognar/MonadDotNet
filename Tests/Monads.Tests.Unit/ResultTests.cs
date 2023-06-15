using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class ResultTests {
  [Fact]
  public void ReturnsSuccessWhenNoErrorIsProvided() {
    Result result = Result.Ok();

    result.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public void ReturnsFailureWhenErrorIsProvided() {
    Exception error = new("test error");
    Result result = Result.Fail(error);

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(error);
  }

  [Fact]
  public void ThrowsExceptionWhenAccessingErrorOnSuccess() {
    Result result = Result.Ok();
    Action act = () => _ = result.Error;

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void FlatMapReturnsSuccessWhenBothResultsAreSuccessful() {
    Result initialResult = Result.Ok();
    Result nextResult = Result.Ok();
    Result TransformToNextResult() => nextResult;

    Result finalResult = initialResult.FlatMap(TransformToNextResult);

    finalResult.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public void FlatMapReturnsFailureWhenInitialResultIsFailure() {
    Result initialResult = Result.Fail(new Exception("initial error"));
    Result nextResult = Result.Ok();
    Result TransformToNextResult() => nextResult;

    Result finalResult = initialResult.FlatMap(TransformToNextResult);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialResult.Error);
  }

  [Fact]
  public void FlatMapReturnsFailureWhenNextResultIsFailure() {
    Result initialResult = Result.Ok();
    Result nextResult = Result.Fail(new Exception("next error"));
    Result TransformToNextResultError() => nextResult;

    Result finalResult = initialResult.FlatMap(TransformToNextResultError);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(nextResult.Error);
  }
}