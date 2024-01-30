namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseTests {
  [Theory]
  [InlineData(0)]
  [InlineData(10)]
  [InlineData(-15)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void ReturnsInternalValueWhenSome(int value) {
    Some(value)
      .OrElse(-1)
      .Should().Be(value);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(10)]
  [InlineData(-15)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void ReturnsFallbackValueWhenNone(int fallback) {
    None<int>()
      .OrElse(fallback)
      .Should().Be(fallback);
  }

  [Fact]
  public void ThrowsExceptionWhenOrElseIsCalledWithNull() {
    Func<string> act = () => Some("str").OrElse((string)null!);
    act.Should().Throw<ArgumentNullException>();
  }
}