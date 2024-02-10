namespace Monads.Tests.Unit.MaybeTests;

public class MaybeFlatMapTests {
  [Property]
  public void MapsValueWhenSome(int value, Func<int, Maybe<NonNull<string>>> f) {
    Maybe.Some(value).FlatMap(f)
      .Should().Be(
        f(value)
      );
  }

  [Property]
  public void PropagatesNoneWhenNone(Func<int, Maybe<NonNull<string>>> f) {
    Maybe.None<int>().FlatMap(f)
      .Should().Be(
        Maybe.None<NonNull<string>>()
      );
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Action act = () => Maybe.Some(10).FlatMap<int>(null!);
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