using System;

namespace Frognar.Monads;

public readonly struct Try<T> {
  readonly T value;
  readonly Exception error;

  public T Value => IsSuccess
    ? value
    : throw new InvalidOperationException("Cannot access the value of a failure.");

  public Exception Error => IsSuccess == false
    ? error
    : throw new InvalidOperationException("Cannot access the exception of a success.");

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

  public Try<U> Map<U>(Func<T, U> f) {
    return IsSuccess ? Try<U>.Success(f(value)) : Try<U>.Failure(error);
  }

  public static Try<T> Success(T value) {
    return new Try<T>(value);
  }

  public static Try<T> Failure(Exception error) {
    return new Try<T>(error);
  }
}