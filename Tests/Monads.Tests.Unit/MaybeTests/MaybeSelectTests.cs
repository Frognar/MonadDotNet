namespace Monads.Tests.Unit.MaybeTests;

public class MaybeSelectTests {

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntToStringTestData))]
  public void MapsValueWhenSome(int value, string expected) {
    Some(value)
      .Select(IntToString)
      .OrElse("")
      .Should().Be(expected);
  }

  [Fact]
  public void PropagatesNoneWhenNone() {
    None<int>()
      .Select(IntToString)
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
  [ClassData(typeof(Helpers.TestDataGenerators.IntPlusOneTestData))]
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