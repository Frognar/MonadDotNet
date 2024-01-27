using System;

namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly bool hasValue;
  readonly T value;

  Maybe(T value) {
    this.value = value ?? throw new ArgumentNullException(nameof(value));
    hasValue = true;
  }

  public T OrElse(T defaultValue) {
    if (defaultValue is null) {
      throw new ArgumentNullException(nameof(defaultValue));
    }

    return hasValue ? value : defaultValue;
  }

  public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) {
    if (selector is null) {
      throw new ArgumentNullException(nameof(selector));
    }

    return SelectMany(x =>
    {
      TResult result = selector(x);
      return result is null ? Maybe<TResult>.None() : Maybe<TResult>.Some(result);
    });
  }

  public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> selector)
    => hasValue ? selector(value) : Maybe<TResult>.None();

  public static Maybe<T> None() => new();
  public static Maybe<T> Some(T value) => new(value);
}