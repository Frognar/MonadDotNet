﻿using System;
using System.Threading.Tasks;

namespace Frognar.Monads;

public readonly struct Result {
  readonly Exception error;
  public bool IsSuccess { get; }
  public Exception Error => IsSuccess ? throw new InvalidOperationException("Result does not contain an error.") : error;

  Result(bool isSuccess) {
    IsSuccess = isSuccess;
    error = null!;
  }
  
  Result(Exception error) {
    IsSuccess = false;
    this.error = error;
  }

  public static Result Ok() => new(true);

  public static Result Fail(Exception error) => new(error);
}

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

  public Result<U> FlatMap<U>(Func<T, Result<U>> f) {
    return IsSuccess ? f(value) : Result<U>.Fail(error);
  }

  public async Task<Result<U>> FlatMapAsync<U>(Func<T, Task<Result<U>>> f) {
    return IsSuccess ? await f(value) : Result<U>.Fail(error);
  }

  public Result<U> Map<U>(Func<T, U> f) {
    return IsSuccess ? Result<U>.Ok(f(value)) : Result<U>.Fail(error);
  }

  public async Task<Result<U>> MapAsync<U>(Func<T, Task<U>> f) {
    return IsSuccess ? Result<U>.Ok(await f(value)) : Result<U>.Fail(error);
  }

  public Result<T> Filter(Func<T, bool> predicate) {
    if (IsSuccess == false) {
      return this;
    }

    return predicate(value) ? this : Fail(new InvalidOperationException("Predicate is not satisfied."));
  }

  public async Task<Result<T>> FilterAsync(Func<T, Task<bool>> predicate) {
    if (IsSuccess == false) {
      return this;
    }

    return await predicate(value) ? this : Fail(new InvalidOperationException("Predicate is not satisfied."));
  }

  public U Match<U>(Func<T, U> onSuccess, Func<Exception, U> onFailure) {
    return IsSuccess ? onSuccess(value) : onFailure(error);
  }

  public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccess, Func<Exception, Task<U>> onFailure) {
    return IsSuccess ? await onSuccess(value) : await onFailure(error);
  }

  public void OnSuccess(Action<T> action) {
    if (IsSuccess) {
      action(value);
    }
  }

  public static Result<T> Ok(T value) => new(value);
  public static Result<T> Fail(Exception error) => new(error);
  
  public static implicit operator Result<T>(T value) => Ok(value);
  public static implicit operator Result<T>(Exception value) => Fail(value);
}