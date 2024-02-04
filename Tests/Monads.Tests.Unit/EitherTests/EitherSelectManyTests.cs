namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectManyTests {
  [Fact]
  public void LeftIdentityLaw() {
    Either<int, string> left = Either<int, string>.Left(42);
    left.SelectMany(Either<int, string>.Left).Should().Be(left);
    Either<int, string> right = Either<int, string>.Right("42");
    right.SelectMany(Either<int, string>.Left).Should().Be(right);
  }
}