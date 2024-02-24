namespace Monads.Tests.Unit.MaybeTests;

public class MaybeFlatMapTests {
  [Property]
  public void MapsValueWhenSome(int value, Func<int, NonNull<string>> f) {
    Maybe.Some(value).FlatMap(v => Maybe.Some(f(v)))
      .Should().Be(
        Maybe.Some(f(value))
      );
  }

  [Property]
  public void PropagatesNoneWhenNone(Func<int, NonNull<string>> f) {
    Maybe.None<int>().FlatMap(v => Maybe.Some(f(v)))
      .Should().Be(
        Maybe.None<NonNull<string>>()
      );
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Func<int, Maybe<string>> f = null!;
    Action act = () => Maybe.Some(10).FlatMap(f);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void CanUseQuerySyntaxForSelectMany(int valueA, int valueB, int valueC) {
    Maybe<int> result =
      from a in Maybe.Some(valueA)
      from b in Maybe.Some(valueB)
      from c in Maybe.Some(valueC)
      select a + b + c;

    result
      .Should().Be(
        Maybe.Some(valueA + valueB + valueC)
      );
  }
}