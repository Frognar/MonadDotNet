namespace Frognar.Monads;

public readonly record struct Either<L, R> {
  readonly IEither either;

  public Either() {
    throw new InvalidOperationException("Default constructor is not allowed for Either<L, R>.");
  }

  Either(IEither either) {
    this.either = either;
  }

  internal static Either<L, R> CreateLeft(L value) => new(new Left(value));
  internal static Either<L, R> CreateRight(R value) => new(new Right(value));

  public TResult Match<TResult>(Func<L, TResult> left, Func<R, TResult> right) {
    ArgumentNullException.ThrowIfNull(left);
    ArgumentNullException.ThrowIfNull(right);
    return either.Match(onLeft: left, onRight: right);
  }

  interface IEither {
    TResult Match<TResult>(Func<L, TResult> onLeft, Func<R, TResult> onRight);
  }

  readonly record struct Left : IEither {
    readonly L left;

    public Left(L left) {
      ArgumentNullException.ThrowIfNull(left);
      this.left = left;
    }

    public TResult Match<TResult>(Func<L, TResult> onLeft, Func<R, TResult> _) => onLeft(left);
  }

  readonly record struct Right : IEither {
    readonly R right;

    public Right(R right) {
      ArgumentNullException.ThrowIfNull(right);
      this.right = right;
    }

    public TResult Match<TResult>(Func<L, TResult> _, Func<R, TResult> onRight) => onRight(right);
  }
}