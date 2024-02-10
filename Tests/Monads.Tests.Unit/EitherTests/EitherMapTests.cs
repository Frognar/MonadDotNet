namespace Monads.Tests.Unit.EitherTests;

public class EitherMapTests {
  [Property]
  public void MapLeftWhenLeftCreated(int value, Func<int, NonNull<string>> f) {
    Either.Left<int, string>(value).LMap(f)
      .Should().Be(
        Either.Left<NonNull<string>, string>(f(value))
      );
  }

  [Property]
  public void PropagateRightWhenRightCreated(NonNull<string> value, Func<int, bool> f) {
    Either.Right<int, string>(value.Get).LMap(f)
      .Should().Be(
        Either.Right<bool, string>(value.Get)
      );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull(int value) {
    Action act = () => Either.Left<int, string>(value).LMap<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapRightWhenRightCreated(NonNull<string> value, Func<string, bool> g) {
    Either.Right<int, string>(value.Get).RMap(g)
      .Should().Be(
        Either.Right<int, bool>(g(value.Get))
      );
  }

  [Property]
  public void PropagateLeftWhenLeftCreated(int value, Func<bool, char> g) {
    Either.Left<int, bool>(value).RMap(g)
      .Should().Be(
        Either.Left<int, char>(value)
      );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull(NonNull<string> value) {
    Action act = () => Either.Right<int, string>(value.Get).RMap<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapLeftInSelectBothWhenLeftCreated(int lValue, Func<int, char> f, Func<string, bool> g) {
    Either.Left<int, string>(lValue).BMap(lMap: f, rMap: g)
      .Should().Be(
        Either.Left<char, bool>(f(lValue))
      );
  }

  [Property]
  public void MapRightInSelectBothWhenRightCreated(NonNull<string> rValue, Func<int, char> f, Func<string, bool> g) {
    Either.Right<int, string>(rValue.Get).BMap(lMap: f, rMap: g)
      .Should().Be(
        Either.Right<char, bool>(g(rValue.Get))
      );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth(int lValue, Func<string, string> g) {
    Action act = () => Either.Left<int, string>(lValue).BMap<int, string>(lMap: null!, rMap: g);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth(char lValue, Func<int, string> f) {
    Action act = () => Either.Right<int, char>(lValue).BMap<string, int>(lMap: f, rMap: null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapRightUsingQuerySyntax(int rValue, Func<int, char> g) {
    Either<int, char> result =
      from right in Either.Right<int, int>(rValue)
      select g(right);

    result
      .Should().Be(
        Either.Right<int, char>(g(rValue))
      );
  }
}