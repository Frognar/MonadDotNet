namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectManyTests {
  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either.Left<int, string>(42)
      .LFlatMap(x => Either.Left<string, string>(x.ToString()))
      .Match(onLeft: left => left, onRight: _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either.Right<int, bool>(true)
      .LFlatMap(x => Either.Left<string, bool>(x.ToString()))
      .Match(onLeft: left => left, onRight: right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").LFlatMap((Func<int, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either.Right<int, string>("42")
      .RFlatMap(x => Either.Right<int, bool>(string.IsNullOrEmpty(x) == false))
      .Match(onLeft: _ => false, onRight: right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either.Left<int, bool>(42)
      .RFlatMap(x => Either.Right<int, int>(x ? 1 : 0))
      .Match(onLeft: _ => -1, onRight: right => right)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull() {
    Action act = () => Either.Right<int, string>("42").RFlatMap((Func<string, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapLeftInSelectBothWhenLeftCreated() {
    Either.Left<int, string>(42)
      .BFlatMap(
        lMap: l => Either.Left<bool, int>(l > 0),
        rMap: r => Either.Right<bool, int>(r.Length)
      )
      .Match(onLeft: left => left, onRight: _ => false)
      .Should().BeTrue();
  }

  [Fact]
  public void MapRightInSelectBothWhenRightCreated() {
    Either.Right<int, string>("42")
      .BFlatMap(
        lMap: l => Either.Left<bool, int>(l > 0),
        rMap: r => Either.Right<bool, int>(r.Length)
      )
      .Match(onLeft: _ => false, onRight: right => right > 0)
      .Should().BeTrue();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth() {
    Action act = () => Either.Left<int, string>(42)
      .BFlatMap(
        lMap: null!,
        rMap: Either.Right<int, string>
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth() {
    Action act = () => Either.Right<int, string>("42")
      .BFlatMap(
        lMap: Either.Left<int, string>,
        rMap: null!
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightUsingQuerySyntax() {
    Either<int, int> result =
      from r1 in Either.Right<int, string>("42")
      from r2 in Either.Right<int, bool>(true)
      select int.Parse(r1) + (r2 ? 100 : 0);

    result.Match(onLeft: _ => -1, onRight: right => right).Should().Be(142);
  }
}