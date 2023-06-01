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
  public bool IsFailure => IsSuccess == false;

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
    T val = value;
    return IsSuccess ? Try<U>.From(() => f(val)) : Try<U>.Failure(error);
  }

  public Try<U> FlatMap<U>(Func<T, Try<U>> f) {
    return IsSuccess ? From(f) : Try<U>.Failure(error);
  }

  public Try<T> Recover(Func<Exception, T> f) {
    return IsSuccess ? this : Success(f(error));
  }

  public Try<T> Filter(Func<T, bool> predicate) {
    if (IsFailure) {
      return this;
    }
    
    return predicate(value) ? this : Failure(new InvalidOperationException("Filter condition not met"));
  }

  public Try<T> Or(Try<T> alternative) {
    return this;
  }

  public TResult Match<TResult>(Func<T, TResult> successFunc, Func<Exception, TResult> errorFunc) {
    return IsSuccess ? successFunc(value) : errorFunc(error);
  }

  Try<U> From<U>(Func<T, Try<U>> f) {
    try {
      return f(value);
    }
    catch (Exception ex) {
      return Try<U>.Failure(ex);
    }
  }

  public static Try<T> From(Func<T> f) {
    try {
      T value = f();
      return Success(value);
    }
    catch (Exception ex) {
      return Failure(ex);
    }
  }

  public static Try<T> Success(T value) {
    return new Try<T>(value);
  }

  public static Try<T> Failure(Exception error) {
    return new Try<T>(error);
  }
}