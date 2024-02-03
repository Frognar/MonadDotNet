using System;

namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly bool hasValue;
  readonly T value;

  public Maybe(T value) {
    ArgumentNullException.ThrowIfNull(value);
    this.value = value;
    hasValue = true;
  }

  public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return SelectMany(x => SomeNullable(selector(x)));
    Maybe<TResult> SomeNullable(TResult x) => x is null ? Maybe<TResult>.None() : Maybe<TResult>.Some(x);
  }

  public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> selector) {
    ArgumentNullException.ThrowIfNull(selector);
    return hasValue ? selector(value) : Maybe<TResult>.None();
  }

  public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) {
    ArgumentNullException.ThrowIfNull(some);
    ArgumentNullException.ThrowIfNull(none);
    return hasValue ? some(value) : none();
  }

  public T OrElse(T defaultValue) {
    ArgumentNullException.ThrowIfNull(defaultValue);
    return hasValue ? value : defaultValue;
  }

  public T OrElse(Func<T> defaultValueFactory) {
    ArgumentNullException.ThrowIfNull(defaultValueFactory);
    return hasValue ? value : defaultValueFactory();
  }

  public static Maybe<T> None() => new();
  public static Maybe<T> Some(T value) => new(value);
}