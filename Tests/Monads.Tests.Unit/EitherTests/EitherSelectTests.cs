namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectTests {
  [Fact]
  public void LeftIdentityLaw() {
    Either<int, string> either = Either<int, string>.Left(42);
    either.SelectLeft(x => x).Should().Be(either);
  }
}