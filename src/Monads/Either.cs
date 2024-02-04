namespace Frognar.Monads;

public readonly record struct Either<L, R> {
  readonly bool isRight;
  readonly L leftValue;
  readonly R rightValue;

  public Either() {
    throw new InvalidOperationException("Default constructor is not allowed for Either<L, R>.");
  }

  Either(L left) {
    ArgumentNullException.ThrowIfNull(left);
    leftValue = left;
    rightValue = default!;
    isRight = false;
  }

  Either(R right) {
    ArgumentNullException.ThrowIfNull(right);
    leftValue = default!;
    rightValue = right;
    isRight = true;
  }

  public static Either<L, R> Left(L value) {
    return new Either<L, R>(value);
  }

  public static Either<L, R> Right(R value) {
    return new Either<L, R>(value);
  }

  public Either<L1, R> SelectLeft<L1>(Func<L, L1> leftSelector) {
    return Either<L1, R>.Left(leftSelector(leftValue));
  }

  public TResult Match<TResult>(Func<L, TResult> left, Func<R, TResult> right) {
    ArgumentNullException.ThrowIfNull(left);
    ArgumentNullException.ThrowIfNull(right);
    return isRight ? right(rightValue) : left(leftValue);
  }
}