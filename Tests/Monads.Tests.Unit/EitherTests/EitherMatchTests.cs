namespace Monads.Tests.Unit.EitherTests;

public class EitherMatchTests {
  [Theory]
  [InlineData(1)]
  [InlineData(100)]
  public void MatchIntOnLeftWhenLeftCreated(int valueOnSuccess) {
    Either.Left<bool, int>(true)
      .Match(
        onLeft: left => left ? valueOnSuccess : 0,
        onRight: _ => -1
      ).Should().Be(valueOnSuccess);
  }

  [Fact]
  public void MatchStringOnLeftWhenLeftCreated() {
    Either.Left<bool, int>(false)
      .Match(
        onLeft: left => left ? "success" : "other kind of success",
        onRight: _ => "Nothing"
      ).Should().Be("other kind of success");
  }

  [Fact]
  public void MatchIntOnRightWhenRightCreated() {
    Either.Right<bool, int>(1)
      .Match(
        onLeft: _ => -1,
        onRight: right => right + 10_000
      ).Should().Be(10_001);
  }

  [Fact]
  public void ThrowsNullArgumentExceptionWhenLeftMethodIsNull() {
    Func<int> act = () => Either.Left<bool, int>(true).Match(onLeft: null!, onRight: _ => 1);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsNullArgumentExceptionWhenRightMethodIsNull() {
    Func<int> act = () => Either.Left<bool, int>(true).Match(onLeft: _ => 1, onRight: null!);
    act.Should().Throw<ArgumentNullException>();
  }
}