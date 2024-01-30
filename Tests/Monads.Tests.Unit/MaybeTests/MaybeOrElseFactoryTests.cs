using System.Diagnostics;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseFactoryTests {
  [Theory]
  [InlineData(0)]
  [InlineData(10)]
  [InlineData(-15)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void ReturnsInternalValueWhenSome(int value) {
    Some(value)
      .OrElse(() => -1)
      .Should().Be(value);
  }

  [Fact]
  public void DefaultFactoryIsNotCalledWhenSome() {
    Some(10)
      .OrElse(() => throw new UnreachableException())
      .Should().Be(10);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(10)]
  [InlineData(-15)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void ReturnsFallbackValueWhenNone(int fallback) {
    None<int>()
      .OrElse(() => fallback)
      .Should().Be(fallback);
  }

  [Fact]
  public void ThrowsExceptionWhenDefaultFactoryIsNull() {
    Func<string> act = () => Some("str").OrElse((Func<string>)null!);
    act.Should().Throw<ArgumentNullException>();
  }
}