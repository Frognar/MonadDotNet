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

  [Fact]
  public async Task FlatMapAsyncReturnsSuccessWhenBothResultsAreSuccessful() {
    Result initialResult = Result.Ok();
    Result nextResult = Result.Ok();

    async Task<Result> TransformToNextResult() {
      await Task.Delay(1);
      return nextResult;
    }

    Result finalResult = await initialResult.FlatMapAsync(TransformToNextResult);

    finalResult.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public async Task FlatMapAsyncReturnsFailureWhenInitialResultIsFailure() {
    Result initialResult = Result.Fail(new Exception("initial error"));
    Result nextResult = Result.Ok();

    async Task<Result> TransformToNextResult() {
      await Task.Delay(1);
      return nextResult;
    }

    Result finalResult = await initialResult.FlatMapAsync(TransformToNextResult);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialResult.Error);
  }

  [Fact]
  public async Task FlatMapAsyncReturnsFailureWhenNextResultIsFailure() {
    Result initialResult = Result.Ok();
    Result nextResult = Result.Fail(new Exception("next error"));

    async Task<Result> TransformToNextResultError() {
      await Task.Delay(1);
      return nextResult;
    }

    Result finalResult = await initialResult.FlatMapAsync(TransformToNextResultError);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(nextResult.Error);
  }

  [Fact]
  public void MatchInvokesSuccessFunctionOnSuccess() {
    Result initialResult = Result.Ok();
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    int OnSuccess() {
      wasSuccessInvoked = true;
      return 1;
    }

    int OnFailure(Exception error) {
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = initialResult.Match(OnSuccess, OnFailure);

    wasSuccessInvoked.Should().BeTrue();
    wasFailureInvoked.Should().BeFalse();
    matchResult.Should().Be(1);
  }

  [Fact]
  public void MatchInvokesFailureFunctionOnFailure() {
    Result initialResult = Result.Fail(new Exception("test error"));
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    int OnSuccess() {
      wasSuccessInvoked = true;
      return 1;
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

  [Fact]
  public async Task MatchAsyncInvokesSuccessFunctionOnSuccess() {
    Result initialResult = Result.Ok();
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    async Task<int> OnSuccess() {
      await Task.Delay(1);
      wasSuccessInvoked = true;
      return 1;
    }

    async Task<int> OnFailure(Exception error) {
      await Task.Delay(1);
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = await initialResult.MatchAsync(OnSuccess, OnFailure);

    wasSuccessInvoked.Should().BeTrue();
    wasFailureInvoked.Should().BeFalse();
    matchResult.Should().Be(1);
  }

  [Fact]
  public async Task MatchAsyncInvokesFailureFunctionOnFailure() {
    Result initialResult = Result.Fail(new Exception("test error"));
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    async Task<int> OnSuccess() {
      await Task.Delay(1);
      wasSuccessInvoked = true;
      return 1;
    }

    async Task<int> OnFailure(Exception error) {
      await Task.Delay(1);
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = await initialResult.MatchAsync(OnSuccess, OnFailure);

    wasSuccessInvoked.Should().BeFalse();
    wasFailureInvoked.Should().BeTrue();
    matchResult.Should().Be(initialResult.Error.Message.Length);
  }

  [Fact]
  public void ImplicitConversionFromExceptionToResult() {
    Exception ex = new("Test exception");
    Result result = ex;

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(ex);
  }
}