namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectTests {
  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either.Left<int, string>(42)
      .SelectLeft(x => x.ToString())
      .Match(left: left => left, right: _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either.Right<int, bool>(true)
      .SelectLeft(x => x.ToString())
      .Match(left: left => left, right: right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull() {
    Action act = () => Either.Left<int, string>(42).SelectLeft<int, int, string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either.Right<int, string>("42")
      .SelectRight(x => string.IsNullOrEmpty(x) == false)
      .Match(left: _ => false, right: right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either.Left<int, bool>(42)
      .SelectRight(x => x ? 1 : 0)
      .Match(left: _ => -1, right: right => right)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").SelectRight<int, string, string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapLeftInSelectBothWhenLeftCreated() {
    Either.Left<int, string>(42)
      .SelectBoth(leftSelector: l => l > 0, rightSelector: _ => "right")
      .Match(left: left => left, right: _ => false)
      .Should().BeTrue();
  }

  [Fact]
  public void MapRightInSelectBothWhenRightCreated() {
    Either.Right<int, string>("42")
      .SelectBoth(leftSelector: l => l > 0, rightSelector: r => r.Length)
      .Match(left: _ => false, right: right => right > 0)
      .Should().BeTrue();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth() {
    Action act = () =>
      Either.Left<int, string>(42).SelectBoth<int, int, string, string>(leftSelector: null!, rightSelector: r => r);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth() {
    Action act = () =>
      Either.Right<int, string>("42").SelectBoth<int, int, string, string>(leftSelector: l => l, rightSelector: null!);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightUsingQuerySyntax() {
    Either<int, string> result =
      from right in Either.Right<int, int>(42)
      select right.ToString();

    result.Match(left: _ => "", right: right => right).Should().Be("42");
  }
}