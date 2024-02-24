using System.Diagnostics;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeMatchTests {
  [Property]
  public void MatchSomeWhenSome(int value, Func<int, string> f, Func<string> g) {
    Maybe.Some(value)
      .Match(
        onSome: f,
        onNone: g
      ).Should().Be(
        f(value)
      );
  }

  [Property]
  public void MatchNoneWhenNone(Func<int, string> f, Func<string> g) {
    Maybe.None<int>()
      .Match(
        onSome: f,
        onNone: g
      ).Should().Be(
        g()
      );
  }

  [Property]
  public void NoneMatchIsNotCalledWhenSome(int value, Func<int, string> f) {
    Func<string> act =
      () => Maybe.Some(value)
        .Match(
          onSome: f,
          onNone: () => throw new UnreachableException()
        );

    act.Should().NotThrow();
  }

  [Property]
  public void SomeMatchIsNotCalledWhenNone(Func<string> g) {
    Func<string> act =
      () => Maybe.None<int>()
        .Match(
          onSome: _ => throw new UnreachableException(),
          onNone: g
        );

    act.Should().NotThrow();
  }

  [Property]
  public void ThrowsExceptionWhenSomeMatchIsNull(Func<string> g) {
    Func<string> act =
      () => Maybe.None<int>()
        .Match(
          onSome: null!,
          onNone: g
        );

    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void ThrowsExceptionWhenNoneMatchIsNull(int value, Func<int, string> f) {
    Func<string> act =
      () => Maybe.Some(value)
        .Match(
          onSome: f,
          onNone: null!
        );

    act.Should().Throw<ArgumentNullException>();
  }
}