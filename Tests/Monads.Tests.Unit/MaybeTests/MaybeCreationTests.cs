namespace Monads.Tests.Unit.MaybeTests;

public class MaybeCreationTests {
  [Property]
  public void ReturnsMaybeWithSome(int value) {
    Maybe.Some(value)
      .Should().Be(
        Maybe.Some(value)
      );
  }

  [Fact]
  public void ReturnsMaybeWithNone() {
    Maybe.None<int>()
      .Should().Be(
        Maybe.None<int>()
      );
  }

  [Fact]
  public void ThrowsExceptionWhenSomeNull() {
    Action act = () => _ = Maybe.Some((string)null!);
    act.Should().Throw<ArgumentNullException>();
  }
}