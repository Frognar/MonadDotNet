namespace Frognar.Monads;

public static class Either {
  public static Either<L, R> Left<L, R>(L value) => Either<L, R>.CreateLeft(value);
  public static Either<L, R> Right<L, R>(R value) => Either<L, R>.CreateRight(value);

  public static Either<L, R1> SelectRight<L, R, R1>(
    this Either<L, R> either,
    Func<R, R1> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return either.Match(left: Left<L, R1>, right: r => Right<L, R1>(selector(r)));
  }

  public static Either<L1, R> SelectLeft<L, L1, R>(
    this Either<L, R> either,
    Func<L, L1> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return either.Match(left: l => Left<L1, R>(selector(l)), right: Right<L1, R>);
  }

  public static Either<L1, R1> SelectBoth<L, L1, R, R1>(
    this Either<L, R> either,
    Func<L, L1> leftSelector,
    Func<R, R1> rightSelector) {
    ArgumentNullException.ThrowIfNull(leftSelector);
    ArgumentNullException.ThrowIfNull(rightSelector);
    return either.Match(left: l => Left<L1, R1>(leftSelector(l)), right: r => Right<L1, R1>(rightSelector(r)));
  }

  public static Either<L, R1> Select<L, R, R1>(
    this Either<L, R> either,
    Func<R, R1> selector)
    => SelectRight(either: either, selector: selector);

  public static Either<L1, R> SelectMany<L, L1, R>(
    this Either<L, R> either,
    Func<L, Either<L1, R>> leftSelector)
    => either.Match(left: leftSelector, right: Right<L1, R>);

  public static Either<L, R1> SelectMany<L, R, R1>(
    this Either<L, R> either,
    Func<R, Either<L, R1>> rightSelector)
    => either.Match(left: Left<L, R1>, right: rightSelector);

  public static Either<L1, R1> SelectMany<L, L1, R, R1>(
    this Either<L, R> either,
    Func<L, Either<L1, R1>> leftSelector,
    Func<R, Either<L1, R1>> rightSelector)
    => either.Match(left: leftSelector, right: rightSelector);

  public static Either<L, R2> SelectMany<L, R, R1, R2>(
    this Either<L, R> source,
    Func<R, Either<L, R1>> firstSelector,
    Func<R, R1, R2> finalSelector)
    => source.SelectMany(x => firstSelector(x).Select(y => finalSelector(arg1: x, arg2: y)));
}