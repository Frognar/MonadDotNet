using Frognar.Monads;

namespace Monads.Tests.Unit; 

public class MaybeTests {
  [Fact]
  public void MaybeWithValue_ShouldHaveValue() {
    const int validValue = 5;

    Maybe<int> maybe = Maybe<int>.From(validValue);
      
    Assert.True(maybe.HasValue);
    Assert.Equal(validValue, maybe.Value);
  }
}