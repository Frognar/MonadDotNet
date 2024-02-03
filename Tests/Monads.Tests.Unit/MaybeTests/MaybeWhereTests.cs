namespace Monads.Tests.Unit.MaybeTests;

public class MaybeWhereTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntFilteredBeLessThanOrEqualTo0TestData))]
  public void FiltersSomeWithPredicate(int value, int expected) {
    Some(value)
      .Where(x => x <= 0)
      .OrElse(Minus1)
      .Should().Be(expected);
  }

  [Fact]
  public void PropagateNoneRegardlessOfPredicate() {
    None<int>()
      .Where(x => x % 2 == 0)
      .OrElse(Minus1)
      .Should().Be(-1);
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntFilteredBeLessThanOrEqualTo0TestData))]
  public void FiltersValueWithQuerySyntax(int value, int expected) {
    Maybe<int> result = 
      from a in Some(value)
      where a <= 0
      select a;
    
    result.OrElse(Minus1)
      .Should().Be(expected);
  }

  [Fact]
  public void PropagateNoneRegardlessOfPredicateWithQuerySyntax() {
    Maybe<int> result = 
      from a in None<int>()
      where a <= 0
      select a;
    
    result.OrElse(Minus1)
      .Should().Be(-1);
  }
}