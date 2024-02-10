using System.Diagnostics;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseFactoryTests {
  [Property]
  public void ReturnsInternalValueWhenSome(int value) {
    Maybe.Some(value).OrElse(() => -1)
      .Should().Be(
        value
      );
  }

  [Property]
  public void DefaultFactoryIsNotCalledWhenSome(int value) {
    Maybe.Some(value).OrElse(() => throw new UnreachableException())
      .Should().Be(
        value
      );
  }

  [Property]
  public void ReturnsFallbackValueWhenNone(Func<int> fallback) {
    Maybe.None<int>().OrElse(fallback)
      .Should().Be(
        fallback()
      );
  }

  [Property]
  public void ThrowsExceptionWhenDefaultFactoryIsNull(NonNull<string> value) {
    Func<string> act = () => Maybe.Some(value.Get).OrElse((Func<string>)null!);
    act.Should().Throw<ArgumentNullException>();
  }
}