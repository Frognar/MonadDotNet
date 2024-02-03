namespace Frognar.Monads;

public static class MaybeExtensions {
  public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe.SelectMany(x => x);

  public static Maybe<TResult> SelectMany<T, U, TResult>(
    this Maybe<T> source,
    Func<T, Maybe<U>> selector,
    Func<T, U, TResult> resultSelector)
    => source.SelectMany(x => selector(x).Select(y => resultSelector(x, y)));
}