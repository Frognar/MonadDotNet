namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseTests {
  [Property]
  public void ReturnsInternalValueWhenSome(int value) {
    Maybe.Some(value).OrElse(-1)
      .Should().Be(
        value
      );
  }

  [Property]
  public void ReturnsFallbackValueWhenNone(int fallback) {
    Maybe.None<int>().OrElse(fallback)
      .Should().Be(
        fallback
      );
  }

  [Fact]
  public void ThrowsExceptionWhenOrElseIsCalledWithNull() {
    Func<string> act = () => Maybe.Some("str").OrElse((string)null!);
    act.Should().Throw<ArgumentNullException>();
  }
}