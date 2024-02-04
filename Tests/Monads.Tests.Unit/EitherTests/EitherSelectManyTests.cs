namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectManyTests {
  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either<int, string>.Left(42)
      .SelectMany(x => Either<string, string>.Left(x.ToString()))
      .Match(left: left => left, right: _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either<int, bool>.Right(true)
      .SelectMany(x => Either<string, bool>.Left(x.ToString()))
      .Match(left: left => left, right: right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull() {
    Action act = () => Either<int, string>.Right("42").SelectMany((Func<int, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either<int, string>.Right("42")
      .SelectMany(x => Either<int, bool>.Right(string.IsNullOrEmpty(x) == false))
      .Match(left: _ => false, right: right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either<int, bool>.Left(42)
      .SelectMany(x => Either<int, int>.Right(x ? 1 : 0))
      .Match(left: _ => -1, right: right => right)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull() {
    Action act = () => Either<int, string>.Right("42").SelectMany((Func<string, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapLeftInSelectBothWhenLeftCreated() {
    Either<int, string>.Left(42)
      .SelectMany(
        leftSelector: l => Either<bool, int>.Left(l > 0),
        r => Either<bool, int>.Right(r.Length)
      )
      .Match(left: left => left, right: _ => false)
      .Should().BeTrue();
  }

  [Fact]
  public void MapRightInSelectBothWhenRightCreated() {
    Either<int, string>.Right("42")
      .SelectMany(
        leftSelector: l => Either<bool, int>.Left(l > 0),
        rightSelector: r => Either<bool, int>.Right(r.Length)
      )
      .Match(left: _ => false, right: right => right > 0)
      .Should().BeTrue();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth() {
    Action act = () => Either<int, string>.Left(42)
      .SelectMany(
        leftSelector: null!,
        rightSelector: Either<int, string>.Right
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth() {
    Action act = () => Either<int, string>.Right("42")
      .SelectMany(
        leftSelector: Either<int, string>.Left,
        rightSelector: null!
      );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapRightUsingQuerySyntax() {
    Either<int, int> result =
      from r1 in Either<int, string>.Right("42")
      from r2 in Either<int, bool>.Right(true)
      select int.Parse(r1) + (r2 ? 100 : 0);

    result.Match(left: _ => -1, right: right => right).Should().Be(142);
  }
}