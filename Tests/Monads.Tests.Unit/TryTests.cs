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
}