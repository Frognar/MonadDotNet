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

  public static Either<L, R> Left(L value) => new(value);
  public static Either<L, R> Right(R value) => new(value);

  public Either<L1, R> SelectLeft<L1>(Func<L, L1> leftSelector) {
    ArgumentNullException.ThrowIfNull(leftSelector);
    return Match(left: l => Either<L1, R>.Left(leftSelector(l)), right: Either<L1, R>.Right);
  }

  public Either<L, R1> SelectRight<R1>(Func<R, R1> rightSelector) {
    ArgumentNullException.ThrowIfNull(rightSelector);
    return Match(left: Either<L, R1>.Left, right: r => Either<L, R1>.Right(rightSelector(r)));
  }

  public Either<L1, R1> SelectBoth<L1, R1>(Func<L, L1> leftSelector, Func<R, R1> rightSelector) {
    ArgumentNullException.ThrowIfNull(leftSelector);
    ArgumentNullException.ThrowIfNull(rightSelector);
    return Match(left: l => Either<L1, R1>.Left(leftSelector(l)), right: r => Either<L1, R1>.Right(rightSelector(r)));
  }

  public Either<L, R1> Select<R1>(Func<R, R1> rightSelector) => SelectRight(rightSelector);

  public Either<L1, R> SelectMany<L1>(Func<L, Either<L1, R>> leftSelector) =>
    Match(left: leftSelector, right: Either<L1, R>.Right);

  public Either<L, R> SelectMany(Func<R, Either<L, R>> rightSelector) => this;

  public TResult Match<TResult>(Func<L, TResult> left, Func<R, TResult> right) {
    ArgumentNullException.ThrowIfNull(left);
    ArgumentNullException.ThrowIfNull(right);
    return isRight ? right(rightValue) : left(leftValue);
  }
}