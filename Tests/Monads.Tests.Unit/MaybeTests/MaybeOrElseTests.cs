namespace Monads.Tests.Unit.MaybeTests;

public class MaybeOrElseTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
  public void ReturnsInternalValueWhenSome(int value) {
    Some(value)
      .OrElse(-1)
      .Should().Be(value);
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
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