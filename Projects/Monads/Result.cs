using System;

namespace Frognar.Monads; 

public readonly struct Result<T> {
  public T Value { get; }
  public Exception Error { get; }
  public bool IsSuccess { get; }
  
  Result(T value) {
    Value = value;
    IsSuccess = true;
    Error = null!;
  }
  
  Result(Exception error) {
    Value = default!;
    IsSuccess = false;
    Error = error;
  }

  public static Result<T> Ok(T value) => new(value);
  public static Result<T> Fail(Exception error) => new(error);
}