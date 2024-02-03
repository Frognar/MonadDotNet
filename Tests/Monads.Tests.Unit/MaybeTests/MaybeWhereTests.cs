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
}