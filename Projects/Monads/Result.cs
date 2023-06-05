﻿using System;

namespace Frognar.Monads; 

public readonly struct Result<T> {
  readonly T value;
  readonly Exception error;
  public T Value => IsSuccess ? value : throw new InvalidOperationException("Result does not contain a value.");
  public Exception Error => IsSuccess ? throw new InvalidOperationException("Result does not contain an error.") : error;
  public bool IsSuccess { get; }
  
  Result(T value) {
    this.value = value;
    IsSuccess = true;
    error = null!;
  }
  
  Result(Exception error) {
    value = default!;
    IsSuccess = false;
    this.error = error;
  }

  public Result<U> Then<U>(Func<T, Result<U>> f) {
    return IsSuccess ? f(value) : Result<U>.Fail(error);
  }

  public Result<U> Map<U>(Func<T, U> f) {
    return IsSuccess ? Result<U>.Ok(f(value)) : Result<U>.Fail(error);
  }

  public U Match<U>(Func<T, U> onSuccess, Func<Exception, U> onFailure) {
    return IsSuccess ? onSuccess(value) : onFailure(error);
  }

  public void OnSuccess(Action<T> action) {
    if (IsSuccess) {
      action(value);
    }
  }

  public static Result<T> Ok(T value) => new(value);
  public static Result<T> Fail(Exception error) => new(error);
}