namespace Frognar.Monads;

public static class MaybeExtensions {
  public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe.SelectMany(x => x);
}