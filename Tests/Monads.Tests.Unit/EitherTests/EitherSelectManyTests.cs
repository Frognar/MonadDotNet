namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectManyTests {
  [Fact]
  public void LeftIdentityLaw() {
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectMany(Either<int, string>.Left).Should().Be(left);
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectMany(Either<int, string>.Left).Should().Be(right);
  }

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
  public void RightIdentityLaw() {
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectMany(Either<int, string>.Right).Should().Be(right);
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectMany(Either<int, string>.Right).Should().Be(left);
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
  public void BothIdentityLaw() {
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectMany(leftSelector: Either<int, string>.Left, rightSelector: Either<int, string>.Right).Should().Be(left);
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectMany(leftSelector: Either<int, string>.Left, rightSelector: Either<int, string>.Right).Should()
      .Be(right);
  }
}