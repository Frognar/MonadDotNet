using System;

namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly T value;
  readonly bool hasValue;

  Maybe(T value) {
    this.value = value ?? throw new ArgumentNullException(nameof(value));
    hasValue = true;
  }

  public T OrElse(T defaultValue) {
    return hasValue ? value : defaultValue;
  }

  public static Maybe<T> None() => new();
  public static Maybe<T> Some(T value) => new(value);
}