using System;

namespace Frognar.Monads; 

public readonly struct Maybe<T> {
  readonly T value = default!;
  public bool HasValue { get; }
  public T Value => HasValue ? value : throw new InvalidOperationException("Cannot access the value of a None.");

  Maybe(T value) {
    this.value = value;
    HasValue = value is not null;
  }

  public Maybe<U> Map<U>(Func<T, U> f) {
    return HasValue ? Maybe<U>.From(f(value)) : Maybe<U>.None;
  }

  public Maybe<T> Filter(Func<T, bool> predicate) {
    return HasValue && predicate(value) ? this : None;
  }

  public Maybe<T> Or(Maybe<T> alternative) {
    return HasValue ? this : alternative;
  }

  public T Or(T alternative) {
    return value;
  }
  
  public static Maybe<T> From(T value) {
    return new Maybe<T>(value);
  }

  public static Maybe<T> None { get; } = new();
}