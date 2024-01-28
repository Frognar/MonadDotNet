using System;

namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly bool hasValue;
  readonly T value;

  Maybe(T value) {
    ArgumentNullException.ThrowIfNull(value);
    this.value = value;
    hasValue = true;
  }

  public T OrElse(T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return hasValue ? value : defaultValue;
  }

  public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return SelectMany(x =>
    {
      TResult result = selector(x);
      return result is null ? Maybe<TResult>.None() : Maybe<TResult>.Some(result);
    });
  }

  public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return hasValue ? selector(value) : Maybe<TResult>.None();
  }

  public static Maybe<T> None() => new();
  public static Maybe<T> Some(T value) => new(value);

  public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) {
    return hasValue ? some(value) : none();
  }
}