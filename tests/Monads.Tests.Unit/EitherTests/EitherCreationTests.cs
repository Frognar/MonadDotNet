namespace Monads.Tests.Unit.EitherTests;

public class EitherCreationTests {
  [Fact]
  public void DefaultConstructionIsNotAllowedForEither() {
    Func<Either<int, int>> act = () => new Either<int, int>();
    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void IsNotNullWhenLeft() {
    Either.Left<int, string>(42).Should().NotBeNull();
    Either.Left<bool, int>(true).Should().NotBeNull();
    Either.Left<string, decimal>("str").Should().NotBeNull();
  }

  [Fact]
  public void IsNotNullWhenRight() {
    Either.Right<int, string>("42").Should().NotBeNull();
    Either.Right<bool, int>(42).Should().NotBeNull();
    Either.Right<string, decimal>(1M).Should().NotBeNull();
  }

  [Fact]
  public void ThrowsArgumentNullExceptionWhenLeftCreatedWithNull() {
    Func<Either<string, int>> act = () => Either.Left<string, int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsArgumentNullExceptionWhenRightCreatedWithNull() {
    Func<Either<int, string>> act = () => Either.Right<int, string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }
}