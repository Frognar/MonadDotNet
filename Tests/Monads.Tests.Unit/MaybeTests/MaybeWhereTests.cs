using Frognar.Monads.Optional;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeWhereTests {
  [Fact]
  public void ReturnsSomeWhenPredicateIsMet() {
    Some(10)
      .Where(x => x < 100)
      .OrElse(Minus1)
      .Should().Be(10);
  }

  [Fact]
  public void ReturnsNoneWhenPredicateIsNotMet() {
    Some(10)
      .Where(x => x > 10)
      .OrElse(Minus1)
      .Should().Be(-1);
  }

  [Fact]
  public void PropagateNoneRegardlessOfPredicate() {
    None<int>()
      .Where(x => x % 2 == 0)
      .OrElse(Minus1)
      .Should().Be(-1);
  }
}