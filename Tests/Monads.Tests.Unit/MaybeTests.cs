using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class MaybeTests {
  [Fact]
  public void ReturnsInternalValueIfCreatedWithValue() {
    Maybe<int> maybe = Maybe<int>.Some(10);
    maybe.OrElse(-1).Should().Be(10);
  }
  
  [Fact]
  public void ReturnsFallbackValueIfCreatedWithoutValue() {
    Maybe<int> maybe = Maybe<int>.None();
    maybe.OrElse(-1).Should().Be(-1);
  }
}