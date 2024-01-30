namespace Monads.Tests.Unit.MaybeTests;

public class MaybeSelectTests {
  static string ToString(int x) => x.ToString();

  [Theory]
  [InlineData(-15, "-15")]
  [InlineData(0, "0")]
  [InlineData(10, "10")]
  [InlineData(int.MaxValue, "2147483647")]
  [InlineData(int.MinValue, "-2147483648")]
  public void MapsValueWhenSome(int value, string expected) {
    Some(value)
      .Select(ToString)
      .OrElse("")
      .Should().Be(expected);
  }

  [Fact]
  public void PropagatesNoneWhenNone() {
    None<int>()
      .Select(ToString)
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void ReturnsNoneWhenSelectorReturnsNull() {
    Some(10)
      .Select(_ => (string?)null)
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Action act = () => Some(10).Select<int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Theory]
  [InlineData(-15, -14)]
  [InlineData(0, 1)]
  [InlineData(10, 11)]
  [InlineData(int.MaxValue, -2147483648)] // overflow
  [InlineData(int.MinValue, -2147483647)]
  public void MapsValuesWithQuerySyntaxWhenSome(int value, int expected) {
    Maybe<int> result =
      from a in Some(value)
      select a + 1;

    result.OrElse(-1)
      .Should().Be(expected);
  }

  [Fact]
  public void PropagatesNoneWithQuerySyntaxWhenNone() {
    Maybe<int> result =
      from a in None<int>()
      select a + 1;

    result.OrElse(-1)
      .Should().Be(-1);
  }
}