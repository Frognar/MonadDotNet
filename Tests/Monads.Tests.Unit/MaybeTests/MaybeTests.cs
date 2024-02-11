namespace Monads.Tests.Unit.MaybeTests;

public class MaybeTests {
  [Property]
  public void FlattensNestedMaybe(int value) {
    Maybe.Some(Maybe.Some(value)).Flatten()
      .Should().Be(
        Maybe.Some(value)
      );
  }

  [Property]
  public void ObeysFirstFunctorLawWhenSome(NonNull<string> value) {
    Maybe.Some(value.Get)
      .Should().Be(
        Maybe.Some(value.Get).Map(x => x)
      );
  }

  [Fact]
  public void ObeysFirstFunctorLawWhenNone() {
    Maybe.None<string>()
      .Should().Be(
        Maybe.None<string>().Map(x => x)
      );
  }

  [Property]
  public void ObeysSecondFunctorLawWhenSome(NonNull<string> value, Func<string, int> f, Func<int, char> g) {
    Maybe.Some(value.Get).Map(f).Map(g)
      .Should().Be(
        Maybe.Some(value.Get).Map(x => g(f(x)))
      );
  }

  [Property]
  public void ObeysSecondFunctorLawWhenNone(Func<string, int> f, Func<int, char> g) {
    Maybe.None<string>().Map(f).Map(g)
      .Should().Be(
        Maybe.None<string>().Map(x => g(f(x)))
      );
  }

  [Property]
  public void ObeysLeftIdentityLaw(int value, Func<int, Maybe<double>> f) {
    Maybe.Some(value).FlatMap(f)
      .Should().Be(
        f(value)
      );
  }

  [Property]
  public void ObeysRightIdentityLaw(NonNull<string> value) {
    Maybe.Some(value.Get).FlatMap(Maybe.Some)
      .Should().Be(
        Maybe.Some(value.Get)
      );
  }

  [Property]
  public void ObeysAssociativityLaw(NonNull<string> value, Func<string, Maybe<int>> f, Func<int, Maybe<double>> g) {
    Maybe.Some(value.Get).FlatMap(f).FlatMap(g)
      .Should().Be(
        Maybe.Some(value.Get).FlatMap(x => f(x).FlatMap(g))
      );
  }
}