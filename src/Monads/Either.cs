namespace Frognar.Monads;

public readonly record struct Either<L, R> {
  readonly L leftValue;

  Either(L left) {
    leftValue = left;
  }

  public static Either<L, R> Left(L value) {
    return new Either<L, R>(value);
  }

  public static Either<L, R> Right(R value) {
    return new Either<L, R>();
  }

  public TResult Match<TResult>(Func<L, TResult> left, Func<R, TResult> right) {
    return left(leftValue);
  }
}