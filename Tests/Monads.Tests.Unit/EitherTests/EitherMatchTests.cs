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

  [Fact]
  public void MatchStringOnLeftWhenLeftCreated() {
    Either<bool, int>.Left(false)
      .Match(
        left => left ? "success" : "other kind of success",
        _ => "Nothing"
      ).Should().Be("other kind of success");
  }
}