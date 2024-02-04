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

  [Fact]
  public void MatchIntOnRightWhenRightCreated() {
    Either<bool, int>.Right(1)
      .Match(
        _ => -1,
        right => right + 10_000
      ).Should().Be(10_001);
  }

  [Fact]
  public void ThrowsNullArgumentExceptionWhenLeftMethodIsNull() {
    Func<int> act = () => Either<bool, int>.Left(true).Match(null!, _ => 1);
    act.Should().Throw<ArgumentNullException>();
  }
}