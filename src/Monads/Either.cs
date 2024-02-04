namespace Frognar.Monads;

public readonly record struct Either<L, R> {
  public static Either<L, R> Left(L value) {
    return new Either<L, R>();
  }
}