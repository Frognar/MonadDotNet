using System;

namespace Frognar.Monads; 

public readonly struct Maybe<T> {
  readonly T value;
  public bool HasValue { get; }
  public T Value => HasValue ? value : throw new InvalidOperationException("Cannot access the value of a None.");

  Maybe(T value) {
    this.value = value;
    HasValue = value is not null;
  }

  public Maybe<U> Map<U>(Func<T, U> f) {
    return Maybe<U>.From(f(value));
  }
  
  public static Maybe<T> From(T value) {
    return new Maybe<T>(value);
  }

  public static Maybe<T> None { get; } = new();
}