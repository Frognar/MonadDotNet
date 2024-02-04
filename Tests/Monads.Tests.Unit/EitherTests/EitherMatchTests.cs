namespace Monads.Tests.Unit.EitherTests;

public class EitherMatchTests {
  [Theory]
  [InlineData(1)]
  [InlineData(100)]
  public void MatchIntOnLeftWhenLeftCreated(int valueOnSuccess) {
    Either<bool, int>.Left(true)
      .Match(
        left => left ? valueOnSuccess : 0,
        _ => -1
      ).Should().Be(valueOnSuccess);
  }
}