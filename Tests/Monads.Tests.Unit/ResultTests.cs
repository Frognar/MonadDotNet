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

  [Fact]
  public void OnSuccessInvokesActionOnSuccess() {
    Result<string> initialResult = Result<string>.Ok("test value");
    bool wasActionInvoked = false;

    void Action(string value) {
      wasActionInvoked = true;
    }

    initialResult.OnSuccess(Action);

    wasActionInvoked.Should().BeTrue();
  }

  [Fact]
  public void OnSuccessDoesNotInvokeActionOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    bool wasActionInvoked = false;

    void Action(string value) {
      wasActionInvoked = true;
    }

    initialResult.OnSuccess(Action);

    wasActionInvoked.Should().BeFalse();
  }

  [Fact]
  public async Task MapAsyncTransformsValueOnSuccess() {
    Result<string> initialResult = Result<string>.Ok("one");
    const int expectedResult = 1;

    async Task<int> AsyncMapFunc(string str) {
      await Task.Delay(1);
      return expectedResult;
    }

    Result<int> mappedResult = await initialResult.MapAsync(AsyncMapFunc);

    mappedResult.IsSuccess.Should().BeTrue();
    mappedResult.Value.Should().Be(expectedResult);
  }

  [Fact]
  public async Task MapAsyncPreservesErrorOnFailure() {
    Exception initialError = new("test error");
    Result<string> initialResult = Result<string>.Fail(initialError);

    async Task<int> AsyncMapFunc(string str) {
      await Task.Delay(1);
      return 1;
    }

    Result<int> mappedResult = await initialResult.MapAsync(AsyncMapFunc);

    mappedResult.IsSuccess.Should().BeFalse();
    mappedResult.Error.Should().Be(initialError);
  }

  [Fact]
  public async Task MapAsyncDoesNotInvokeFunctionOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    bool wasInvoked = false;

    async Task<int> AsyncMapFunc(string str) {
      wasInvoked = true;
      await Task.Delay(1);
      return 1;
    }

    _ = await initialResult.MapAsync(AsyncMapFunc);

    wasInvoked.Should().BeFalse();
  }

  [Fact]
  public void ImplicitConversionFromTToResultProducesSuccess() {
    const string value = "test value";
    Result<string> result = value;

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void ImplicitConversionFromExceptionToResultProducesFailure() {
    Exception error = new("test error");
    Result<string> result = error;

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(error);
  }

  [Fact]
  public async Task ThenAsyncReturnsSuccessWhenBothResultsAreSuccessful() {
    Result<string> initialResult = Result<string>.Ok("test value");
    Result<int> nextResult = Result<int>.Ok(123);

    async Task<Result<int>> AsyncTransformToInt(string _) {
      await Task.Delay(1);
      return nextResult;
    }

    Result<int> finalResult = await initialResult.ThenAsync(AsyncTransformToInt);

    finalResult.IsSuccess.Should().BeTrue();
    finalResult.Value.Should().Be(nextResult.Value);
  }

  [Fact]
  public async Task ThenAsyncReturnsFailureWhenInitialResultIsFailure() {
    Exception initialError = new("initial error");
    Result<string> initialResult = Result<string>.Fail(initialError);
    Result<int> nextResult = Result<int>.Ok(123);

    async Task<Result<int>> AsyncTransformToInt(string _) {
      await Task.Delay(1);
      return nextResult;
    }

    Result<int> finalResult = await initialResult.ThenAsync(AsyncTransformToInt);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialError);
  }

  [Fact]
  public async Task ThenAsyncReturnsFailureWhenNextResultIsFailure() {
    Result<string> initialResult = Result<string>.Ok("test value");
    Exception nextError = new("next error");
    Result<int> nextResult = Result<int>.Fail(nextError);

    async Task<Result<int>> AsyncTransformToIntError(string _) {
      await Task.Delay(1);
      return nextResult;
    }

    Result<int> finalResult = await initialResult.ThenAsync(AsyncTransformToIntError);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(nextError);
  }

  [Fact]
  public async Task MatchAsyncInvokesSuccessFunctionOnSuccess() {
    Result<string> initialResult = Result<string>.Ok("test value");
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    async Task<int> OnSuccessAsync(string value) {
      await Task.Delay(1);
      wasSuccessInvoked = true;
      return value.Length;
    }

    async Task<int> OnFailureAsync(Exception error) {
      await Task.Delay(1);
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = await initialResult.MatchAsync(OnSuccessAsync, OnFailureAsync);

    wasSuccessInvoked.Should().BeTrue();
    wasFailureInvoked.Should().BeFalse();
    matchResult.Should().Be(initialResult.Value.Length);
  }

  [Fact]
  public async Task MatchAsyncInvokesFailureFunctionOnFailure() {
    Result<string> initialResult = Result<string>.Fail(new Exception("test error"));
    bool wasSuccessInvoked = false;
    bool wasFailureInvoked = false;

    async Task<int> OnSuccessAsync(string value) {
      await Task.Delay(1);
      wasSuccessInvoked = true;
      return value.Length;
    }

    async Task<int> OnFailureAsync(Exception error) {
      await Task.Delay(1);
      wasFailureInvoked = true;
      return error.Message.Length;
    }

    int matchResult = await initialResult.MatchAsync(OnSuccessAsync, OnFailureAsync);

    wasSuccessInvoked.Should().BeFalse();
    wasFailureInvoked.Should().BeTrue();
    matchResult.Should().Be(initialResult.Error.Message.Length);
  }

  [Fact]
  public void FilterPreservesSuccessWhenPredicateIsTrue() {
    Result<string> initialResult = Result<string>.Ok("test value");
    bool Predicate(string value) => value == "test value";

    Result<string> finalResult = initialResult.Filter(Predicate);

    finalResult.IsSuccess.Should().BeTrue();
    finalResult.Value.Should().Be(initialResult.Value);
  }

  [Fact]
  public void FilterConvertsToFailureWhenPredicateIsFalse() {
    Result<string> initialResult = Result<string>.Ok("test value");
    bool Predicate(string value) => value != "test value";

    Result<string> finalResult = initialResult.Filter(Predicate);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().BeOfType<InvalidOperationException>();
  }

  [Fact]
  public void FilterPreservesFailureRegardlessOfPredicate() {
    Exception initialError = new("initial error");
    Result<string> initialResult = Result<string>.Fail(initialError);
    bool Predicate(string value) => true;

    Result<string> finalResult = initialResult.Filter(Predicate);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialError);
  }

  [Fact]
  public async Task FilterAsyncPreservesSuccessWhenPredicateIsTrue() {
    Result<string> initialResult = Result<string>.Ok("test value");

    async Task<bool> PredicateAsync(string value) {
      await Task.Delay(1);
      return value == "test value";
    }

    Result<string> finalResult = await initialResult.FilterAsync(PredicateAsync);

    finalResult.IsSuccess.Should().BeTrue();
    finalResult.Value.Should().Be(initialResult.Value);
  }

  [Fact]
  public async Task FilterAsyncConvertsToFailureWhenPredicateIsFalse() {
    Result<string> initialResult = Result<string>.Ok("test value");

    async Task<bool> PredicateAsync(string value) {
      await Task.Delay(1);
      return value != "test value";
    }

    Result<string> finalResult = await initialResult.FilterAsync(PredicateAsync);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().BeOfType<InvalidOperationException>();
  }

  [Fact]
  public async Task FilterAsyncPreservesFailureRegardlessOfPredicate() {
    Exception initialError = new Exception("initial error");
    Result<string> initialResult = Result<string>.Fail(initialError);

    async Task<bool> PredicateAsync(string value) {
      await Task.Delay(1);
      return true;
    }

    Result<string> finalResult = await initialResult.FilterAsync(PredicateAsync);

    finalResult.IsSuccess.Should().BeFalse();
    finalResult.Error.Should().Be(initialError);
  }
}