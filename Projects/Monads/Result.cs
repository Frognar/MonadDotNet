using System;

namespace Frognar.Monads {
  public readonly struct Result<T> : Monad<T> {
    public T Value { get; }
    public Exception? Error { get; }
    public bool IsSuccess { get; }

    Result(T value, Exception? error) {
      Value = value;
      Error = error;
      IsSuccess = error == null;
    }
    
    public static Result<T> Success(T value) => new Result<T>(value, null);
    public static Result<T> Failure(Exception error) => new Result<T>(default!, error);
    public Monad<U> Bind<U>(Func<T, Monad<U>> func) {
      return IsSuccess ? func(Value) : Result<U>.Failure(Error!);
    }

    public static implicit operator Result<T>(T result) => Success(result);
    public static implicit operator Result<T>(Exception error) => Failure(error);
  }
}