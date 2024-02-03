namespace Frognar.Monads.Results;

public static class ResultExtensions {
  public static Result<T> ToResult<T>(this T value) {
    return Result<T>.Ok(value);
  }

  public static Result<T> ToResult<T>(this IEnumerable<Error> errors) {
    return Result<T>.Fail(errors);
  }

  public static Result<T> ToResult<T>(this Error error) {
    return Result<T>.Fail(error);
  }

  public static Result<T> ToResult<T>(this Exception exception) {
    return Result<T>.Fail(Error.Failure("Failure", exception.Message));
  }
}