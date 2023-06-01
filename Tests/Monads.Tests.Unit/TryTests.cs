using Frognar.Monads;

namespace Monads.Tests.Unit;

public class TryTests {
  [Fact]
  public void SuccessMethod_CreatesTryWithCorrectValue_WhenGivenValidValue() {
    const int validValue = 5;

    Try<int> tryResult = Try<int>.Success(validValue);

    Assert.True(tryResult.IsSuccess);
    Assert.Equal(validValue, tryResult.Value);
  }

  [Fact]
  public void FailureMethod_CreatesTryInFailureState_WhenCalledWithException() {
    Exception exception = new("Test exception");

    Try<int> tryResult = Try<int>.Failure(exception);

    Assert.False(tryResult.IsSuccess);
    Assert.Equal(exception, tryResult.Error);
    Assert.Throws<InvalidOperationException>(() => { _ = tryResult.Value; });
  }

  [Fact]
  public void ExceptionProperty_ThrowsException_WhenTryIsSuccess() {
    const int validValue = 5;

    Try<int> tryResult = Try<int>.Success(validValue);

    Assert.Throws<InvalidOperationException>(() => { _ = tryResult.Error; });
  }

  [Fact]
  public void MapMethod_TransformsValue_WhenTryIsSuccess() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> mappedResult = tryResult.Map(value => value.ToString());

    Assert.True(mappedResult.IsSuccess);
    Assert.Equal(validValue.ToString(), mappedResult.Value);
  }

  [Fact]
  public void MapMethod_DoesNotTransformValue_WhenTryIsFailure() {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    Try<string> mappedResult = tryResult.Map(value => value.ToString());

    Assert.False(mappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = mappedResult.Value; });
    Assert.Equal(exception, mappedResult.Error);
  }

  [Fact]
  public void MapMethod_ReturnsFailure_WhenFunctionThrowsException() {
    const int validValue = 5;
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> mappedResult = tryResult.Map<string>(_ => throw exception);

    Assert.False(mappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = mappedResult.Value; });
    Assert.Equal(exception, mappedResult.Error);
  }

  [Fact]
  public void FlatMapMethod_TransformsValue_WhenTryIsSuccess() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> flatMappedResult = tryResult.FlatMap(value => Try<string>.Success(value.ToString()));

    Assert.True(flatMappedResult.IsSuccess);
    Assert.Equal(validValue.ToString(), flatMappedResult.Value);
  }

  [Fact]
  public void FlatMapMethod_DoesNotTransformValue_WhenTryIsFailure() {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    Try<string> flatMappedResult = tryResult.FlatMap(value => Try<string>.Success(value.ToString()));

    Assert.False(flatMappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = flatMappedResult.Value; });
    Assert.Equal(exception, flatMappedResult.Error);
  }

  [Fact]
  public void FlatMapMethod_ReturnsFailure_WhenFunctionThrowsException() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Exception exception = new("Test exception");
    Try<string> Func(int _) => throw exception;
    Try<string> flatMappedResult = tryResult.FlatMap(Func);

    Assert.False(flatMappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = flatMappedResult.Value; });
    Assert.Equal(exception, flatMappedResult.Error);
  }

  [Fact]
  public void RecoverMethod_DoesNotChangeValue_WhenTryIsSuccess() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<int> recoveredResult = tryResult.Recover(_ => 0);

    Assert.True(recoveredResult.IsSuccess);
    Assert.Equal(validValue, recoveredResult.Value);
  }

  [Fact]
  public void RecoverMethod_TransformsFailureIntoSuccess() {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    const int recoverValue = 0;
    Try<int> recoveredResult = tryResult.Recover(_ => recoverValue);

    Assert.True(recoveredResult.IsSuccess);
    Assert.Equal(recoverValue, recoveredResult.Value);
  }

  [Fact]
  public void RecoverMethod_PropagatesException_WhenRecoverFunctionThrows() {
    Exception initialException = new Exception("Initial exception");
    Exception recoverException = new Exception("Recover exception");
    Try<int> tryResult = Try<int>.Failure(initialException);

    void Act() => tryResult.Recover(_ => throw recoverException);

    Exception ex = Assert.Throws<Exception>(Act);
    Assert.Equal(recoverException, ex);
  }

  [Fact]
  public void Filter_PropagatesValue_WhenTryIsSuccessAndPredicateIsTrue() {
    const int validValue = 5;
    const int smallerValidValue = validValue - 1;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<int> filteredResult = tryResult.Filter(value => value > smallerValidValue);

    Assert.True(filteredResult.IsSuccess);
    Assert.Equal(validValue, filteredResult.Value);
  }

  [Fact]
  public void Filter_ReturnsFailure_WhenTryIsSuccessAndPredicateIsFalse() {
    const int validValue = -5;
    const int biggerValidValue = validValue + 1;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<int> filteredResult = tryResult.Filter(value => value > biggerValidValue);

    Assert.True(filteredResult.IsFailure);
    Assert.Throws<InvalidOperationException>(() => { _ = filteredResult.Value; });
  }

  [Fact]
  public void Filter_PropagatesError_WhenTryIsFailure() {
    Exception initialException = new("Initial exception");
    Try<int> tryResult = Try<int>.Failure(initialException);

    Try<int> filteredResult = tryResult.Filter(value => value > 0);

    Assert.True(filteredResult.IsFailure);
    Assert.Equal(initialException, filteredResult.Error);
  }

  [Fact]
  public void Filter_PropagatesException_WhenPredicateThrowsException() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);
    Exception exception = new("Test exception");

    void Act() => tryResult.Filter(_ => throw exception);
    
    Exception ex = Assert.Throws<Exception>(Act);
    Assert.Equal(exception, ex);
  }
  
  [Fact]
  public void Match_ReturnsCorrectValue_WhenTryIsSuccess() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    string matchResult = tryResult.Match(
      successValue => $"Success: {successValue}",
      _ => "Failure"
    );

    Assert.Equal($"Success: {validValue}", matchResult);
  }

  [Fact]
  public void Match_ReturnsCorrectValue_WhenTryIsFailure() {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    string matchResult = tryResult.Match(
      _ => "Success",
      failureError => $"Failure: {failureError.Message}"
    );

    Assert.Equal($"Failure: {exception.Message}", matchResult);
  }
  
  [Fact]
  public void Or_ReturnsSameTry_WhenTryIsSuccess() {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);
    Try<int> alternative = Try<int>.Success(validValue + 1);

    Try<int> orResult = tryResult.Or(alternative);

    Assert.True(orResult.IsSuccess);
    Assert.Equal(validValue, orResult.Value);
  }
}