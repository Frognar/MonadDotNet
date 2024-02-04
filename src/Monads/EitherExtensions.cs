namespace Frognar.Monads;

public static class EitherExtensions {
  public static Either<L, R2> SelectMany<L, R, R1, R2>(
    this Either<L, R> source,
    Func<R, Either<L, R1>> firstSelector,
    Func<R, R1, R2> finalSelector)
    => source.SelectMany(x => firstSelector(x).Select(y => finalSelector(x, y)));
}