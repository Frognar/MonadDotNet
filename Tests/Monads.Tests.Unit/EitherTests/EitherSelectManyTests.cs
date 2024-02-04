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
}