namespace Monads.Tests.Unit.EitherTests;

public class EitherMatchTests {
  [Fact]
  public void MatchOnLeftWhenLeftCreated() {
    Either<bool, int>.Left(true)
      .Match(
        left => left ? 1 : 0,
        right => -1
      ).Should().Be(1);
  }
}