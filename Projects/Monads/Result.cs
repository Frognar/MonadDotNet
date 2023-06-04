using System;

namespace Frognar.Monads; 

public readonly struct Result<T> {
  readonly T value;
  public T Value => IsSuccess ? value : throw new InvalidOperationException("Result does not contain a value.");
  public Exception Error { get; }
  public bool IsSuccess { get; }
  
  Result(T value) {
    this.value = value;
    IsSuccess = true;
    Error = null!;
  }
  
  Result(Exception error) {
    value = default!;
    IsSuccess = false;
    Error = error;
  }

  public static Result<T> Ok(T value) => new(value);
  public static Result<T> Fail(Exception error) => new(error);
}