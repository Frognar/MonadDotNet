namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectTests {
  [Fact]
  public void LeftIdentityLaw() {
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectLeft(x => x).Should().Be(left);
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectLeft(x => x).Should().Be(right);
  }

  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either<int, string>.Left(42)
      .SelectLeft(x => x.ToString())
      .Match(left => left, _ => "")
      .Should().Be("42");
  }

  [Fact]
  public void PropagateRightWhenRightCreated() {
    Either<int, bool>.Right(true)
      .SelectLeft(x => x.ToString())
      .Match(left => left, right => right ? "true" : "false")
      .Should().Be("true");
  }

  [Fact]
  public void RightIdentityLaw() {
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectRight(x => x).Should().Be(right);
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectRight(x => x).Should().Be(left);
  }

  [Fact]
  public void MapRightWhenRightCreated() {
    Either<int, string>.Right("42")
      .SelectRight(x => string.IsNullOrEmpty(x) == false)
      .Match(_ => false, right => right)
      .Should().Be(true);
  }

  [Fact]
  public void PropagateLeftWhenLeftCreated() {
    Either<int, bool>.Left(42)
      .SelectRight(x => x ? 1 : 0)
      .Match(_ => -1, right => right)
      .Should().Be(-1);
  }
}