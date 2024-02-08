namespace Frognar.Monads;

public static class Maybe {
  public static Maybe<T> None<T>() => Maybe<T>.CreateNone();
  public static Maybe<T> Some<T>(T value) => Maybe<T>.CreateSome(value);

  public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return maybe.Match(onSome: x => Some(selector(x)), onNone: None<TResult>);
  }

  public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> maybe, Func<T, Maybe<TResult>> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return maybe.Match(onSome: selector, onNone: None<TResult>);
  }

  public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate) {
    return maybe.Match(onSome: x => predicate(x) ? maybe : None<T>(), onNone: () => maybe);
  }

  public static T OrElse<T>(this Maybe<T> maybe, T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return maybe.Match(onSome: x => x, onNone: () => defaultValue);
  }

  public static T OrElse<T>(this Maybe<T> maybe, Func<T> defaultValueFactory) {
    ArgumentNullException.ThrowIfNull(defaultValueFactory);
    return maybe.Match(onSome: x => x, onNone: defaultValueFactory);
  }

  public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe.SelectMany(x => x);

  public static Maybe<TResult> SelectMany<T, U, TResult>(
    this Maybe<T> source,
    Func<T, Maybe<U>> selector,
    Func<T, U, TResult> resultSelector)
    => source.SelectMany(x => selector(x).Select(y => resultSelector(arg1: x, arg2: y)));
}