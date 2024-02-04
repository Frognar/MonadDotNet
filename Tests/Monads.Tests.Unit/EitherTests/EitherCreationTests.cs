namespace Monads.Tests.Unit.EitherTests;

public class EitherCreationTests {
  [Fact]
  public void IsNotNullWhenLeft() {
    Either<int, string>.Left(42).Should().NotBeNull();
    Either<bool, int>.Left(true).Should().NotBeNull();
    Either<string, decimal>.Left("str").Should().NotBeNull();
  }

  [Fact]
  public void IsNotNullWhenRight() {
    Either<int, string>.Right("42").Should().NotBeNull();
    Either<bool, int>.Right(42).Should().NotBeNull();
    Either<string, decimal>.Right(1M).Should().NotBeNull();
  }

  [Fact]
  public void ThrowsArgumentNullExceptionWhenLeftCreatedWithNull() {
    var act = () => Either<string, int>.Left(null!);
    act.Should().Throw<ArgumentNullException>();
  }
}