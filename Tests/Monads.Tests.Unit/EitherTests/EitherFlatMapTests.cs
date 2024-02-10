namespace Monads.Tests.Unit.EitherTests;

public class EitherFlatMapTests {
  [Property]
  public void MapLeftWhenLeftCreated(int lValue, Func<int, char> f) {
    Either.Left<int, char>(lValue).LFlatMap(l => Either.Left<int, char>(f(l)))
      .Should().Be(
        Either.Left<int, char>(f(lValue))
      );
  }

  [Property]
  public void PropagateRightWhenRightCreated(char rValue, Func<int, char> f) {
    Either.Right<int, char>(rValue).LFlatMap(l => Either.Left<int, char>(f(l)))
      .Should().Be(
        Either.Right<int, char>(rValue)
      );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNull(NonNull<string> rValue) {
    Action act = () => Either.Right<int, string>(rValue.ToString()).LFlatMap((Func<int, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapRightWhenRightCreated(NonNull<string> rValue, Func<string, char> g) {
    Either.Right<int, string>(rValue.Get).RFlatMap(r => Either.Right<int, char>(g(r)))
      .Should().Be(
        Either.Right<int, char>(g(rValue.Get))
      );
  }

  [Property]
  public void PropagateLeftWhenLeftCreated(int lValue, Func<char, string> g) {
    Either.Left<int, char>(lValue).RFlatMap(r => Either.Right<int, string>(g(r)))
      .Should().Be(
        Either.Left<int, string>(lValue)
      );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNull(NonNull<string> rValue) {
    Action act = () => Either.Right<int, string>(rValue.Get).RFlatMap((Func<string, Either<int, string>>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapLeftInSelectBothWhenLeftCreated(int lValue, Func<int, char> f, Func<string, int> g) {
    Either.Left<int, string>(lValue)
      .BFlatMap(lMap: l => Either.Left<char, int>(f(l)), rMap: r => Either.Right<char, int>(g(r)))
      .Should().Be(
        Either.Left<char, int>(f(lValue))
      );
  }

  [Property]
  public void MapRightInSelectBothWhenRightCreated(NonNull<string> rValue, Func<int, char> f, Func<string, int> g) {
    Either.Right<int, string>(rValue.Get).BFlatMap(
      lMap: l => Either.Left<char, int>(f(l)),
      rMap: r => Either.Right<char, int>(g(r))
    ).Should().Be(
      Either.Right<char, int>(g(rValue.Get))
    );
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenLeftSelectorIsNullInSelectBoth(int lValue) {
    Action act = () => Either.Left<int, string>(lValue).BFlatMap(
      lMap: null!,
      rMap: Either.Right<int, string>
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void ThrowArgumentNullExceptionWhenRightSelectorIsNullInSelectBoth(NonNull<string> rValue) {
    Action act = () => Either.Right<int, string>(rValue.Get).BFlatMap(
      lMap: Either.Left<int, string>,
      rMap: null!
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Property]
  public void MapRightUsingQuerySyntax(NonNull<string> rValue1, bool rValue2) {
    Either<int, string> result =
      from r1 in Either.Right<int, string>(rValue1.Get)
      from r2 in Either.Right<int, bool>(rValue2)
      select $"{r1}:{r2}";

    result
      .Should().Be(
        Either.Right<int, string>($"{rValue1.Get}:{rValue2}")
      );
  }
}