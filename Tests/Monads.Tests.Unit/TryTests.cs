using Frognar.Monads;

namespace Monads.Tests.Unit;

public class TryTests {
  [Fact]
  public void SuccessMethod_CreatesTryWithCorrectValue_WhenGivenValidValue()
  {
    const int validValue = 5;

    Try<int> tryResult = Try<int>.Success(validValue);

    Assert.True(tryResult.IsSuccess);
    Assert.Equal(validValue, tryResult.Value);
  }
  [Fact]
  public void FailureMethod_CreatesTryInFailureState_WhenCalledWithException()
  {
    Exception exception = new("Test exception");

    Try<int> tryResult = Try<int>.Failure(exception);

    Assert.False(tryResult.IsSuccess);
    Assert.Equal(exception, tryResult.Error);
    Assert.Throws<InvalidOperationException>(() => { _ = tryResult.Value; });
  }
  
  [Fact]
  public void ExceptionProperty_ThrowsException_WhenTryIsSuccess()
  {
    const int validValue = 5;

    Try<int> tryResult = Try<int>.Success(validValue);

    Assert.Throws<InvalidOperationException>(() => { _ = tryResult.Error; });
  }
  
  [Fact]
  public void MapMethod_TransformsValue_WhenTryIsSuccess()
  {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> mappedResult = tryResult.Map(value => value.ToString());

    Assert.True(mappedResult.IsSuccess);
    Assert.Equal(validValue.ToString(), mappedResult.Value);
  }

  [Fact]
  public void MapMethod_DoesNotTransformValue_WhenTryIsFailure()
  {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    Try<string> mappedResult = tryResult.Map(value => value.ToString());

    Assert.False(mappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = mappedResult.Value; });
    Assert.Equal(exception, mappedResult.Error);
  }
  
  [Fact]
  public void MapMethod_ReturnsFailure_WhenFunctionThrowsException()
  {
    const int validValue = 5;
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> mappedResult = tryResult.Map<string>(_ => throw exception);

    Assert.False(mappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = mappedResult.Value; });
    Assert.Equal(exception, mappedResult.Error);
  }
  
  [Fact]
  public void FlatMapMethod_TransformsValue_WhenTryIsSuccess()
  {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> flatMappedResult = tryResult.FlatMap(value => Try<string>.Success(value.ToString()));

    Assert.True(flatMappedResult.IsSuccess);
    Assert.Equal(validValue.ToString(), flatMappedResult.Value);
  }

  [Fact]
  public void FlatMapMethod_DoesNotTransformValue_WhenTryIsFailure()
  {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    Try<string> flatMappedResult = tryResult.FlatMap(value => Try<string>.Success(value.ToString()));

    Assert.False(flatMappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = flatMappedResult.Value; });
    Assert.Equal(exception, flatMappedResult.Error);
  }

  [Fact]
  public void FlatMapMethod_ReturnsFailure_WhenFunctionThrowsException()
  {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<string> Func(int _) => throw new Exception("Test exception");
    Try<string> flatMappedResult = tryResult.FlatMap(Func);

    Assert.False(flatMappedResult.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => { _ = flatMappedResult.Value; });
    Assert.Equal("Test exception", flatMappedResult.Error.Message);
  }
  
  [Fact]
  public void RecoverMethod_DoesNotChangeValue_WhenTryIsSuccess()
  {
    const int validValue = 5;
    Try<int> tryResult = Try<int>.Success(validValue);

    Try<int> recoveredResult = tryResult.Recover(ex => 0);

    Assert.True(recoveredResult.IsSuccess);
    Assert.Equal(validValue, recoveredResult.Value);
  }

  [Fact]
  public void RecoverMethod_TransformsFailureIntoSuccess() {
    Exception exception = new("Test exception");
    Try<int> tryResult = Try<int>.Failure(exception);

    const int recoverValue = 0;
    Try<int> recoveredResult = tryResult.Recover(ex => recoverValue);

    Assert.True(recoveredResult.IsSuccess);
    Assert.Equal(recoverValue, recoveredResult.Value);
  }
}