namespace Frognar.Monads;

public static class Maybe {
  public static Maybe<T> None<T>() => Maybe<T>.CreateNone();
  public static Maybe<T> Some<T>(T value) => Maybe<T>.CreateSome(value);

  public static Maybe<TResult> Map<T, TResult>(this Maybe<T> source, Func<T, TResult> map) {
    ArgumentNullException.ThrowIfNull(map);
    return source.Match(
      onNone: Maybe<TResult>.CreateNone,
      onSome: value => Maybe<TResult>.CreateSome(map(value))
    );
  }

  public static Maybe<TResult> Select<T, TResult>(this Maybe<T> source, Func<T, TResult> selector)
    => source.Map(selector);

  public static Maybe<T> Where<T>(this Maybe<T> source, Func<T, bool> predicate) {
    ArgumentNullException.ThrowIfNull(predicate);
    return source.Match(onSome: x => predicate(x) ? source : None<T>(), onNone: () => source);
  }

  public static T OrElse<T>(this Maybe<T> source, T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return source.Match(onSome: x => x, onNone: () => defaultValue);
  }

  public static T OrElse<T>(this Maybe<T> source, Func<T> defaultValueFactory) {
    ArgumentNullException.ThrowIfNull(defaultValueFactory);
    return source.Match(onSome: x => x, onNone: defaultValueFactory);
  }

  public static Maybe<TResult> FlatMap<T, TResult>(this Maybe<T> source, Func<T, Maybe<TResult>> map) {
    ArgumentNullException.ThrowIfNull(map);
    return source.Match(
      onNone: Maybe<TResult>.CreateNone,
      onSome: map
    );
  }

  public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> source) => source.FlatMap(x => x);

  public static Maybe<TResult> SelectMany<T, U, TResult>(
    this Maybe<T> source,
    Func<T, Maybe<U>> maybeSelector,
    Func<T, U, TResult> selector)
    => source.FlatMap(x => maybeSelector(x).Map(y => selector(arg1: x, arg2: y)));
}