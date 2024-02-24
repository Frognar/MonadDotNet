namespace Monads.Tests.Unit.MaybeTests;

public class MaybeMapTests {

  [Property]
  public void MapsValueWhenSome(int value, Func<int, decimal> f) {
    Maybe.Some(value).Map(f)
      .Should().Be(
        Maybe.Some(f(value))
      );
  }

  [Property]
  public void PropagatesNoneWhenNone(Func<int, string> f) {
    Maybe.None<int>().Map(f)
      .Should().Be(
        Maybe.None<string>()
      );
  }

  [Fact]
  public void ThrowsWhenSelectorReturnsNull() {
    Action act = () => Maybe.Some(10).Map(_ => (string)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Func<int, string> f = null!;
    Action act = () => Maybe.Some(10).Map(f);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void CanUseQuerySyntaxForSelect(NonNull<string> value, Func<string, int> f) {
    IMaybe<int> result =
      from a in Maybe.Some(value.Get)
      select f(a);

    result
      .Should().Be(
        Maybe.Some(f(value.Get))
      );
  }
}