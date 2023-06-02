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
  
  [Fact]
  public void Map_WithNonNullValue_TransformsValue() {
    const int originalValue = 5;

    Maybe<int> maybe = Maybe<int>.From(originalValue);
    Maybe<string> mapped = maybe.Map(value => value.ToString());

    Assert.True(mapped.HasValue);
    Assert.Equal(originalValue.ToString(), mapped.Value);
  }

  [Fact]
  public void Map_WithNone_DoesNotTransform() {
    Maybe<int> maybe = Maybe<int>.None;
    Maybe<string> mapped = maybe.Map(value => value.ToString());

    Assert.False(mapped.HasValue);
    Assert.Throws<InvalidOperationException>(() => { _ = mapped.Value; });
  }
}
