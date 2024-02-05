namespace Monads.Tests.Unit.EitherTests;

public class EitherMatchTests {
  [Theory]
  [InlineData(1)]
  [InlineData(100)]
  public void MatchIntOnLeftWhenLeftCreated(int valueOnSuccess) {
    Either.Left<bool, int>(true)
      .Match(
        left: left => left ? valueOnSuccess : 0,
        right: _ => -1
      ).Should().Be(valueOnSuccess);
  }

  [Fact]
  public void MatchStringOnLeftWhenLeftCreated() {
    Either.Left<bool, int>(false)
      .Match(
        left: left => left ? "success" : "other kind of success",
        right: _ => "Nothing"
      ).Should().Be("other kind of success");
  }

  [Fact]
  public void MatchIntOnRightWhenRightCreated() {
    Either.Right<bool, int>(1)
      .Match(
        left: _ => -1,
        right: right => right + 10_000
      ).Should().Be(10_001);
  }

  [Fact]
  public void ThrowsNullArgumentExceptionWhenLeftMethodIsNull() {
    Func<int> act = () => Either.Left<bool, int>(true).Match(left: null!, right: _ => 1);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsNullArgumentExceptionWhenRightMethodIsNull() {
    Func<int> act = () => Either.Left<bool, int>(true).Match(left: _ => 1, right: null!);
    act.Should().Throw<ArgumentNullException>();
  }
}