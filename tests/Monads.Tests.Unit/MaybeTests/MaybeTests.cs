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
  public void ObeysLeftIdentityLaw(int value, Func<int, double> f) {
    IMaybe<double> F(int x) => Maybe.Some(f(x));
    Maybe.Some(value).FlatMap(F)
      .Should().Be(
        F(value)
      );
  }

  [Property]
  public void ObeysRightIdentityLaw(NonNull<string> value) {
    Maybe.Some(value.Get).FlatMap(v => (IMaybe<string>)Maybe.Some(v))
      .Should().Be(
        Maybe.Some(value.Get)
      );
  }

  [Property]
  public void ObeysAssociativityLaw(NonNull<string> value, Func<string, int> f, Func<int, double> g) {
    IMaybe<int> F(string x) => Maybe.Some(f(x));
    IMaybe<double> G(int x) => Maybe.Some(g(x));
    Maybe.Some(value.Get).FlatMap(F).FlatMap(G)
      .Should().Be(
        Maybe.Some(value.Get).FlatMap(x => F(x).FlatMap(G))
      );
  }
}