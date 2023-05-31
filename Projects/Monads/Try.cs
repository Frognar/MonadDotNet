using System;

namespace Frognar.Monads;

public readonly struct Try<T> {
  readonly T value;
  readonly Exception error;

  public T Value => IsSuccess ? value : throw new InvalidOperationException("Cannot access the value of a failure.");
  public Exception Error => error;
  public bool IsSuccess { get; }

  Try(T value) {
    this.value = value;
    error = null!;
    IsSuccess = true;
  }

  Try(Exception error) {
    this.error = error;
    value = default!;
    IsSuccess = false;
  }

  public static Try<T> Success(T value) {
    return new Try<T>(value);
  }

  public static Try<int> Failure(Exception error) {
    return new Try<int>(error);
  }
}