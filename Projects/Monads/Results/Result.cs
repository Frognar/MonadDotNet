using System;
using System.Collections.Generic;

namespace Frognar.Monads.Results;

public readonly struct Result<T> where T : class {
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
  
  public static Result<T> Ok(T obj) {
    return new Result<T>(obj);
  }
  
  public static Result<T> Fail(Error error) {
    return new Result<T>(error);
  }
  
  public static Result<T> Fail(List<Error> errors) {
    return new Result<T>(errors);
  }
  
  public Result<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return isError ? Result<TResult>.Fail(errors!) : Result<TResult>.Ok(map(value!));
  }
}