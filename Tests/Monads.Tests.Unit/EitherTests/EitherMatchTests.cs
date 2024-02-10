namespace Monads.Tests.Unit.EitherTests;

public class EitherMatchTests {
  [Property]
  public void MatchIntOnLeftWhenLeftCreated(NonNull<string> lValue, Func<string, int> f, Func<int, int> g) {
    Either.Left<string, int>(lValue.Get).Match(onLeft: f, onRight: g)
      .Should().Be(
        f(lValue.Get)
      );
  }

  [Property]
  public void MatchStringOnLeftWhenLeftCreated(NonNull<string> lValue, Func<string, string> f, Func<int, string> g) {
    Either.Left<string, int>(lValue.Get).Match(onLeft: f, onRight: g)
      .Should().Be(
        f(lValue.Get)
      );
  }

  [Property]
  public void MatchIntOnRightWhenRightCreated(int rValue, Func<bool, int> f, Func<int, int> g) {
    Either.Right<bool, int>(rValue).Match(onLeft: f, onRight: g)
      .Should().Be(
        g(rValue)
      );
  }

  [Property]
  public void ThrowsNullArgumentExceptionWhenLeftMethodIsNull(Func<int, int> g) {
    Func<int> act = () => Either.Left<bool, int>(true).Match(onLeft: null!, onRight: g);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void ThrowsNullArgumentExceptionWhenRightMethodIsNull(Func<bool, int> f) {
    Func<int> act = () => Either.Left<bool, int>(true).Match(onLeft: f, onRight: null!);
    act.Should().Throw<ArgumentNullException>();
  }
}