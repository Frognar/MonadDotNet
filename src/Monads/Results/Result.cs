﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Frognar.Monads.Results;

public readonly record struct Result<T> {
  readonly T? value;
  readonly ImmutableList<Error> errors;
  bool HasErrors => errors.IsEmpty == false;

  Result(T value) {
    this.value = value;
    errors = ImmutableList<Error>.Empty;
  }

  Result(IEnumerable<Error> errors) {
    value = default;
    this.errors = errors.ToImmutableList();
  }

  public static Result<T> Ok(T obj) => new(obj);
  public static Result<T> Fail(Error error) => new([error]);
  public static Result<T> Fail(IEnumerable<Error> errors) => new(errors);

  public static Result<T> Try(Func<T> func) {
    try {
      return Ok(func());
    }
    catch (Exception exception) {
      return Fail(Error.Failure(description: exception.Message));
    }
  }

  public async static Task<Result<T>> TryAsync(Func<Task<T>> func) {
    try {
      T result = await func().ConfigureAwait(false);
      return Ok(result);
    }
    catch (Exception exception) {
      return Fail(Error.Failure(description: exception.Message));
    }
  }

  public Result<TResult> Map<TResult>(Func<T, TResult> map) {
    if (HasErrors) {
      return Result<TResult>.Fail(errors);
    }

    T localValue = value!;
    return Result<TResult>.Try(() => map(localValue));
  }

  public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) {
    if (HasErrors) {
      return Result<TResult>.Fail(errors);
    }

    T localValue = value!;
    return await Result<TResult>.TryAsync(() => map(localValue)).ConfigureAwait(false);
  }

  public Result<TResult> FlatMap<TResult>(Func<T, Result<TResult>> map) {
    return HasErrors
      ? Result<TResult>.Fail(errors)
      : map(value!);
  }

  public async Task<Result<TResult>> FlatMapAsync<TResult>(Func<T, Task<Result<TResult>>> map) {
    return HasErrors
      ? Result<TResult>.Fail(errors)
      : await map(value!).ConfigureAwait(false);
  }

  public void Switch(
    Action<T> onSuccess,
    Action<IEnumerable<Error>> onFailure) {
    if (HasErrors) {
      onFailure(errors);
    }
    else {
      onSuccess(value!);
    }
  }

  public async Task SwitchAsync(
    Func<T, Task> onSuccess,
    Func<IEnumerable<Error>, Task> onFailure) {
    if (HasErrors) {
      await onFailure(errors);
    }
    else {
      await onSuccess(value!);
    }
  }

  public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
    Func<IEnumerable<Error>, TResult> onFailure) {
    return HasErrors
      ? onFailure(errors)
      : onSuccess(value!);
  }

  public async Task<TResult> MatchAsync<TResult>(
    Func<T, Task<TResult>> onSuccess,
    Func<IEnumerable<Error>, Task<TResult>> onFailure) {
    return HasErrors
      ? await onFailure(errors)
      : await onSuccess(value!);
  }
  
  public static implicit operator Result<T>(T value) => Ok(value);
  public static implicit operator Result<T>(Error error) => Fail(error);
}