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
    public static Result<T> Of(Func<T> func) {
      try {
        return Success(func());
      }
      catch (Exception e) {
        return Failure(e);
      }
    }

    public Monad<U> Bind<U>(Func<T, Monad<U>> func) {
      if (IsSuccess == false) {
        return Result<U>.Failure(Error!);
      }

      try {
        return func(Value);
      }
      catch (Exception e) {
        return Result<U>.Failure(e);
      }
    }
  }
}