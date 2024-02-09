using FsCheck;
using FsCheck.Xunit;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeWhereTests {
  [Property]
  public Property PropagateSomeWhenPredicateIsMeet(int value) {
    int result = Some(value)
      .Where(v => v > 100)
      .OrElse(-1);

    return (result == value)
      .When(value > 100)
      .Collect($"{value}");
  }

  [Property]
  public Property IsNoneWhenPredicateIsNotMeet(int value) {
    int result = Some(value)
      .Where(v => v > 100)
      .OrElse(-1);

    return (result == -1)
      .When(value <= 100)
      .Collect($"{value}");
  }

  [Property]
  public Property PropagateNoneRegardlessOfPredicate(Func<int, bool> predicate) {
    int result = None<int>()
      .Where(predicate)
      .OrElse(-1);

    return (result == -1).ToProperty();
  }

  [Fact]
  public void ThrowsWhenPredicateIsNull() {
    Action act = () => None<int>().Where(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public Property CanUseQuerySyntaxForWhere(int value) {
    Maybe<int> maybe =
      from a in Some(value)
      where a > 100
      select a;

    int result = maybe
      .OrElse(-1);

    return (result == value)
      .When(value > 100)
      .Collect($"{value}");
  }
}