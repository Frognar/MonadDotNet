using System;

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

  public static Result<T> Ok(T value) => new(value);
  public static Result<T> Fail(Exception error) => new(error);
}