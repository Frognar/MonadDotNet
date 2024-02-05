using Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeSelectManyTests {
  [Theory]
  [ClassData(typeof(IntToStringTestData))]
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
    Action act = () => Some(10).SelectMany<int, int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Theory]
  [ClassData(typeof(ThreeIntSumTestData))]
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