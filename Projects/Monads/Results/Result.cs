using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frognar.Monads.Results;

public readonly struct Result<T> {
  readonly T? value;
  readonly List<Error>? errors;
  readonly bool isError;

  Result(T value) {
    this.value = value;
    errors = null;
    isError = false;
  }

  Result(List<Error> errors) {
    value = default;
    this.errors = errors;
    isError = true;
  }

  Result(Error error) {
    value = default;
    errors = [error];
    isError = true;
  }

  public static Result<T> Ok(T obj) => new(obj);
  public static Result<T> Fail(Error error) => new(error);
  public static Result<T> Fail(List<Error> errors) => new(errors);

  public Result<TResult> Map<TResult>(Func<T, TResult> map) {
    return isError ? Result<TResult>.Fail(errors!) : Result<TResult>.Ok(map(value!));
  }

  public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) {
    return isError ? Result<TResult>.Fail(errors!) : Result<TResult>.Ok(await map(value!).ConfigureAwait(false));
  }

  public Result<TResult> FlatMap<TResult>(Func<T, Result<TResult>> map) {
    return isError ? Result<TResult>.Fail(errors!) : map(value!);
  }

  public async Task<Result<TResult>> FlatMapAsync<TResult>(Func<T, Task<Result<TResult>>> map) {
    return isError ? Result<TResult>.Fail(errors!) : await map(value!).ConfigureAwait(false);
  }

  public void Switch(Action<T> onSuccess, Action<List<Error>> onFailure) {
    if (isError) {
      onFailure(errors!);
    }
    else {
      onSuccess(value!);
    }
  }

  public async Task SwitchAsync(Func<T, Task> onSuccess, Func<List<Error>, Task> onFailure) {
    if (isError) {
      await onFailure(errors!);
    }
    else {
      await onSuccess(value!);
    }
  }

  public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<List<Error>, TResult> onFailure) {
    return isError ? onFailure(errors!) : onSuccess(value!);
  }

  public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess,
    Func<List<Error>, Task<TResult>> onFailure) {
    return isError ? await onFailure(errors!) : await onSuccess(value!);
  }
}