namespace Frognar.Monads;

public static class Either {
  public static Either<L, R> Left<L, R>(L value) => Either<L, R>.CreateLeft(value);
  public static Either<L, R> Right<L, R>(R value) => Either<L, R>.CreateRight(value);
  public static Either<L, R1> Select<L, R, R1>(this Either<L, R> source, Func<R, R1> selector) => source.RMap(selector);

  public static Either<L, R2> SelectMany<L, R, R1, R2>(
    this Either<L, R> source,
    Func<R, Either<L, R1>> eitherSelector,
    Func<R, R1, R2> selector)
    => source.RFlatMap(x => eitherSelector(x).Select(y => selector(arg1: x, arg2: y)));
}