using System;
using Frognar.Monads.Enums;

namespace Frognar.Monads {
  public readonly struct Result<T> {
    readonly ResultState state;
    readonly T value;
    readonly Exception error;
    
    public bool IsSuccess => state == ResultState.Success;
    public bool IsFailure => state == ResultState.Failure;

    public Result(T value) {
      state = ResultState.Success;
      this.value = value;
      error = null!;
    }

    public Result(Exception ex) {
      state = ResultState.Failure;
      error = ex;
      value = default;
    }

    public override string ToString() {
      return IsSuccess
        ? value?.ToString() ?? "(null)"
        : error?.ToString() ?? "(unknown error)";
    }
    
    public U Match<U>(Func<T, U> success, Func<Exception, U> failure) {
      return IsSuccess ? success(value) : failure(error);
    }
    
    public void Resolve(Action<T> success, Action<Exception> failure) {
      if (IsSuccess) {
        success(value);
      }
      else {
        failure(error);
      }
    }

    public Result<U> Map<U>(Func<T, Result<U>> func) {
      return IsSuccess ? func(value) : new Result<U>(error);
    }

    public static Result<T> Of(T value) => new(value);
    public static implicit operator Result<T>(T result) => new(result);
    public static implicit operator Result<T>(Exception error) => new(error);
  }
}