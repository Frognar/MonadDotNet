namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectTests {
  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either.Left<int, string>(42)
      .LMap(x => x.ToString())
      .Match(onLeft: left => left, onRight: _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either.Right<int, bool>(true)
      .LMap(x => x.ToString())
      .Match(onLeft: left => left, onRight: right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull() {
    Action act = () => Either.Left<int, string>(42).LMap<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either.Right<int, string>("42")
      .RMap(x => string.IsNullOrEmpty(x) == false)
      .Match(onLeft: _ => false, onRight: right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either.Left<int, bool>(42)
      .RMap(x => x ? 1 : 0)
      .Match(onLeft: _ => -1, onRight: right => right)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").RMap<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapLeftInSelectBothWhenLeftCreated() {
    Either.Left<int, string>(42)
      .BMap(lMap: l => l > 0, rMap: _ => "right")
      .Match(onLeft: left => left, onRight: _ => false)
      .Should().BeTrue();
  }

  [Fact]
  public void MapRightInSelectBothWhenRightCreated() {
    Either.Right<int, string>("42")
      .BMap(lMap: l => l > 0, rMap: r => r.Length)
      .Match(onLeft: _ => false, onRight: right => right > 0)
      .Should().BeTrue();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth() {
    Action act = () =>
      Either.Left<int, string>(42).BMap<int, string>(lMap: null!, rMap: r => r);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth() {
    Action act = () =>
      Either.Right<int, string>("42").BMap<int, string>(lMap: l => l, rMap: null!);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightUsingQuerySyntax() {
    Either<int, string> result =
      from right in Either.Right<int, int>(42)
      select right.ToString();

    result.Match(onLeft: _ => "", onRight: right => right).Should().Be("42");
  }
}