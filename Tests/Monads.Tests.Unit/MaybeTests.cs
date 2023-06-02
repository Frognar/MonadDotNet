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

  [Fact]
  public void NoneMethod_ReturnsMaybeNone() {
    Maybe<int> maybe = Maybe<int>.None;

    Assert.False(maybe.HasValue);
    Assert.Throws<InvalidOperationException>(() => { _ = maybe.Value; });
  }

  [Fact]
  public void FromMethod_WithNullValue_ReturnsMaybeNone() {
    Maybe<object> maybe = Maybe<object>.From(null!);

    Assert.False(maybe.HasValue);
    Assert.Throws<InvalidOperationException>(() => { _ = maybe.Value; });
  }
}
