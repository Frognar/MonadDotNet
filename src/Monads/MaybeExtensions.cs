namespace Frognar.Monads;

public static class Maybe {
  public static Maybe<T> None<T>() => Maybe<T>.CreateNone();
  public static Maybe<T> Some<T>(T value) => Maybe<T>.CreateSome(value);

  public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return maybe.SelectMany(x => SomeNullable(selector(x)));
    Maybe<TResult> SomeNullable(TResult x) => x is null ? None<TResult>() : Some<TResult>(x);
  }

  public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> maybe, Func<T, Maybe<TResult>> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return maybe.Match(some: selector, none: None<TResult>);
  }

  public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate) {
    return maybe.Match(some: x => predicate(x) ? maybe : None<T>(), none: () => maybe);
  }

  public static T OrElse<T>(this Maybe<T> maybe, T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return maybe.Match(some: x => x, none: () => defaultValue);
  }

  public static T OrElse<T>(this Maybe<T> maybe, Func<T> defaultValueFactory) {
    ArgumentNullException.ThrowIfNull(defaultValueFactory);
    return maybe.Match(some: x => x, none: defaultValueFactory);
  }

  public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe.SelectMany(x => x);

  public static Maybe<TResult> SelectMany<T, U, TResult>(
    this Maybe<T> source,
    Func<T, Maybe<U>> selector,
    Func<T, U, TResult> resultSelector)
    => source.SelectMany(x => selector(x).Select(y => resultSelector(arg1: x, arg2: y)));
}