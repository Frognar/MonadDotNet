namespace Frognar.Monads;

public static class Maybe {
  public static Maybe<T> None<T>() => Maybe<T>.CreateNone();
  public static Maybe<T> Some<T>(T value) => Maybe<T>.CreateSome(value);

  public static Maybe<TResult> Map<T, TResult>(this IMaybe<T> source, Func<T, TResult> map) {
    ArgumentNullException.ThrowIfNull(map);
    return source.Match(
      onNone: None<TResult>,
      onSome: value => Some(map(value))
    );
  }

  public static IMaybe<TResult> Select<T, TResult>(this IMaybe<T> source, Func<T, TResult> selector)
    => source.Map(selector);

  public static Maybe<T> Where<T>(this Maybe<T> source, Func<T, bool> predicate) {
    ArgumentNullException.ThrowIfNull(predicate);
    return source.Match(onSome: x => predicate(x) ? source : None<T>(), onNone: () => source);
  }

  public static T OrElse<T>(this IMaybe<T> source, T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return source.Match(onSome: x => x, onNone: () => defaultValue);
  }

  public static T OrElse<T>(this IMaybe<T> source, Func<T> defaultValueFactory) {
    ArgumentNullException.ThrowIfNull(defaultValueFactory);
    return source.Match(onSome: x => x, onNone: defaultValueFactory);
  }

  public static Maybe<TResult> FlatMap<T, TResult>(this IMaybe<T> source, Func<T, Maybe<TResult>> map) {
    ArgumentNullException.ThrowIfNull(map);
    return source.Match(
      onNone: None<TResult>,
      onSome: map
    );
  }

  public static IMaybe<TResult> FlatMap<T, TResult>(this IMaybe<T> source, Func<T, IMaybe<TResult>> map) {
    ArgumentNullException.ThrowIfNull(map);
    return source.Match(
      onNone: () => None<TResult>(),
      onSome: map
    );
  }

  public static Maybe<T> Flatten<T>(this IMaybe<Maybe<T>> source) => source.FlatMap(x => x);

  public static IMaybe<TResult> SelectMany<T, U, TResult>(
    this IMaybe<T> source,
    Func<T, IMaybe<U>> maybeSelector,
    Func<T, U, TResult> selector)
    => source.FlatMap(x => maybeSelector(x).Map(y => selector(arg1: x, arg2: y)));
}