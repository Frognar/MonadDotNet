namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseFactoryTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
  public void ReturnsInternalValueWhenSome(int value) {
    Some(value)
      .OrElse(Minus1)
      .Should().Be(value);
  }

  [Fact]
  public void DefaultFactoryIsNotCalledWhenSome() {
    Some(10)
      .OrElse(ThrowUnreachable)
      .Should().Be(10);
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
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