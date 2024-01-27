using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class MaybeTests {
  [Fact]
  public void ReturnsInternalValueIfCreatedWithValue() {
    Maybe<int> maybe = new(10);
    maybe.OrElse(-1).Should().Be(10);
  }
}