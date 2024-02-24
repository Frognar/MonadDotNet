namespace Monads.Tests.Unit.MaybeTests;

public class MaybeWhereTests {
  [Property]
  public void PropagateSomeWhenPredicateIsMeet(NonNegativeInt value) {
    Maybe.Some(value.Get).Where(v => v >= 0)
      .Should().Be(
        Maybe.Some(value.Get)
      );
  }

  [Property]
  public void IsNoneWhenPredicateIsNotMeet(NegativeInt value) {
    Maybe.Some(value.Get).Where(v => v >= 0)
      .Should().Be(
        Maybe.None<int>()
      );
  }

  [Property]
  public void PropagateNoneRegardlessOfPredicate(Func<int, bool> predicate) {
    Maybe.None<int>().Where(predicate)
      .Should().Be(
        Maybe.None<int>()
      );
  }

  [Fact]
  public void ThrowsWhenPredicateIsNull() {
    Action act = () => Maybe.None<int>().Where(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void CanUseQuerySyntaxForWhere(NonNegativeInt value) {
    IMaybe<int> maybe =
      from a in Maybe.Some(value.Get)
      where a >= 0
      select a;

    maybe
      .Should().Be(
        Maybe.Some(value.Get)
      );
  }
}