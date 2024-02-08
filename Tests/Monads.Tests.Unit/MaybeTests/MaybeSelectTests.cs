using Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeSelectTests {

  [Theory]
  [ClassData(typeof(IntToStringTestData))]
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
  public void ThrowsWhenSelectorReturnsNull() {
    Action act = () => Some(10).Select(_ => (string)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Action act = () => Some(10).Select<int, int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Theory]
  [ClassData(typeof(IntPlusOneTestData))]
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