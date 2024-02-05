namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectManyTests {
  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either.Left<int, string>(42)
      .SelectMany(x => Either.Left<string, string>(x.ToString()))
      .Match(left: left => left, right: _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either.Right<int, bool>(true)
      .SelectMany(x => Either.Left<string, bool>(x.ToString()))
      .Match(left: left => left, right: right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").SelectMany((Func<int, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either.Right<int, string>("42")
      .SelectMany(x => Either.Right<int, bool>(string.IsNullOrEmpty(x) == false))
      .Match(left: _ => false, right: right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either.Left<int, bool>(42)
      .SelectMany(x => Either.Right<int, int>(x ? 1 : 0))
      .Match(left: _ => -1, right: right => right)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").SelectMany((Func<string, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapLeftInSelectBothWhenLeftCreated() {
    Either.Left<int, string>(42)
      .SelectMany(
        leftSelector: l => Either.Left<bool, int>(l > 0),
        rightSelector: r => Either.Right<bool, int>(r.Length)
      )
      .Match(left: left => left, right: _ => false)
      .Should().BeTrue();
  }

  [Fact]
  public void MapRightInSelectBothWhenRightCreated() {
    Either.Right<int, string>("42")
      .SelectMany(
        leftSelector: l => Either.Left<bool, int>(l > 0),
        rightSelector: r => Either.Right<bool, int>(r.Length)
      )
      .Match(left: _ => false, right: right => right > 0)
      .Should().BeTrue();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth() {
    Action act = () => Either.Left<int, string>(42)
      .SelectMany(
        leftSelector: null!,
        rightSelector: Either.Right<int, string>
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth() {
    Action act = () => Either.Right<int, string>("42")
      .SelectMany(
        leftSelector: Either.Left<int, string>,
        rightSelector: null!
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightUsingQuerySyntax() {
    Either<int, int> result =
      from r1 in Either.Right<int, string>("42")
      from r2 in Either.Right<int, bool>(true)
      select int.Parse(r1) + (r2 ? 100 : 0);

    result.Match(left: _ => -1, right: right => right).Should().Be(142);
  }
}