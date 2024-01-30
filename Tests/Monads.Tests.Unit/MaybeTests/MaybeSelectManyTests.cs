namespace Monads.Tests.Unit.MaybeTests;

public class MaybeSelectManyTests {
  static Maybe<string> ToMaybeString(int x) => Some(x.ToString());

  [Theory]
  [InlineData(-15, "-15")]
  [InlineData(0, "0")]
  [InlineData(10, "10")]
  [InlineData(int.MaxValue, "2147483647")]
  [InlineData(int.MinValue, "-2147483648")]
  public void MapsValueWhenSome(int value, string expected) {
    Some(value)
      .SelectMany(ToMaybeString)
      .OrElse("none")
      .Should().Be(expected);
  }

  [Fact]
  public void PropagatesNoneWhenNone() {
    None<int>()
      .SelectMany(ToMaybeString)
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Action act = () => Some(10).SelectMany<int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Theory]
  [InlineData(-15, 0, 15, 0)]
  [InlineData(0, 1, 2, 3)]
  [InlineData(10, 11, -153, -132)]
  [InlineData(int.MaxValue, +1, -1, int.MaxValue)]
  [InlineData(int.MaxValue, +1, 0, int.MinValue)] // overflow
  [InlineData(int.MinValue, -1, -1, 2147483646)] // underflow
  public void MapsValuesWithQuerySyntaxWhenSome(int valueA, int valueB, int valueC, int expected) {
    Maybe<int> result =
      from a in Some(valueA)
      from b in Some(valueB)
      from c in Some(valueC)
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(expected);
  }

  [Fact]
  public void PropagatesNoneWithQuerySyntaxWhenNone() {
    Maybe<int> result =
      from a in Some(1)
      from b in Some(2)
      from c in None<int>()
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(-1);
  }
}