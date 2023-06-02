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

  [Fact]
  public void Filter_WithMatchingValue_ReturnsJust() {
    const int originalValue = 5;

    Maybe<int> maybe = Maybe<int>.From(originalValue);
    Maybe<int> filtered = maybe.Filter(_ => true);

    Assert.True(filtered.HasValue);
    Assert.Equal(originalValue, filtered.Value);
  }

  [Fact]
  public void Filter_WithNonMatchingValue_ReturnsNone() {
    const int originalValue = 5;

    Maybe<int> maybe = Maybe<int>.From(originalValue);
    Maybe<int> filtered = maybe.Filter(_ => false);

    Assert.False(filtered.HasValue);
    Assert.Throws<InvalidOperationException>(() => { _ = filtered.Value; });
  }

  [Fact]
  public void Filter_WithNone_ReturnsNone() {
    Maybe<string> maybe = Maybe<string>.None;
    Maybe<string> filtered = maybe.Filter(value => value.Length > 0);

    Assert.False(filtered.HasValue);
    Assert.Throws<InvalidOperationException>(() => { _ = filtered.Value; });
  }

  [Fact]
  public void Or_WithJust_DoesNotReturnAlternative() {
    const int originalValue = 5;
    const int alternativeValue = 10;

    Maybe<int> maybe = Maybe<int>.From(originalValue);
    Maybe<int> result = maybe.Or(Maybe<int>.From(alternativeValue));

    Assert.True(result.HasValue);
    Assert.Equal(originalValue, result.Value);
  }

  [Fact]
  public void Or_WithNone_ReturnsAlternative() {
    Maybe<int> maybe = Maybe<int>.None;
    const int alternativeValue = 5;

    Maybe<int> result = maybe.Or(Maybe<int>.From(alternativeValue));

    Assert.True(result.HasValue);
    Assert.Equal(alternativeValue, result.Value);
  }
}
