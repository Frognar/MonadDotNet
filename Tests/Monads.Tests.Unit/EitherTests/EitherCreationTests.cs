namespace Monads.Tests.Unit.EitherTests;

public class EitherCreationTests {
  [Fact]
  public void IsNotNullWhenLeft() {
    Either<int, string>.Left(42).Should().NotBeNull();
    Either<bool, int>.Left(true).Should().NotBeNull();
    Either<string, decimal>.Left("str").Should().NotBeNull();
  }
}