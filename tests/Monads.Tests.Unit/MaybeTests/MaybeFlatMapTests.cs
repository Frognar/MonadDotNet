namespace Monads.Tests.Unit.MaybeTests;

public class MaybeFlatMapTests {
  [Property]
  public void MapsValueWhenSome(int value, Func<int, NonNull<string>> f) {
    IMaybe<string> F(int x) => Maybe.Some(f(x).Get);
    Maybe.Some(value).FlatMap(F)
      .Should().Be(
        F(value)
      );
  }

  [Property]
  public void PropagatesNoneWhenNone(Func<int, NonNull<string>> f) {
    IMaybe<string> F(int x) => Maybe.Some(f(x).Get);
    Maybe.None<int>().FlatMap(F)
      .Should().Be(
        Maybe.None<string>()
      );
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Func<int, IMaybe<string>> f = null!;
    Action act = () => Maybe.Some(10).FlatMap(f);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void CanUseQuerySyntaxForSelectMany(int valueA, int valueB, int valueC) {
    IMaybe<int> result =
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