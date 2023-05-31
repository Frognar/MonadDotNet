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
    Exception exception = new Exception("Test exception");

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
}