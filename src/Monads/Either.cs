namespace Frognar.Monads;

public readonly record struct Either<L, R> {
  public static Either<L, R> Left(L value) {
    return new Either<L, R>();
  }

  public static Either<L, R> Right(R value) {
    return new Either<L, R>();
  }

  public int Match(Func<L, int> left, Func<R, int> right) {
    return 1;
  }
}