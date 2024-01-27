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

  public static Maybe<T> None() => new();
  public static Maybe<T> Some(T value) => new(value);
}