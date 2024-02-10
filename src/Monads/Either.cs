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

  public TResult Match<TResult>(Func<L, TResult> onLeft, Func<R, TResult> onRight) {
    ArgumentNullException.ThrowIfNull(onLeft);
    ArgumentNullException.ThrowIfNull(onRight);
    return either.Match(onLeft: onLeft, onRight: onRight);
  }

  public Either<L, R1> RMap<R1>(Func<R, R1> rMap) {
    ArgumentNullException.ThrowIfNull(rMap);
    return either.Match(onLeft: Either<L, R1>.CreateLeft, onRight: r => Either<L, R1>.CreateRight(rMap(r)));
  }

  public Either<L1, R> LMap<L1>(Func<L, L1> lMap) {
    ArgumentNullException.ThrowIfNull(lMap);
    return either.Match(onLeft: l => Either<L1, R>.CreateLeft(lMap(l)), onRight: Either<L1, R>.CreateRight);
  }

  public Either<L1, R1> BMap<L1, R1>(Func<L, L1> lMap, Func<R, R1> rMap) {
    ArgumentNullException.ThrowIfNull(lMap);
    ArgumentNullException.ThrowIfNull(rMap);
    return either.Match(
      onLeft: l => Either<L1, R1>.CreateLeft(lMap(l)),
      onRight: r => Either<L1, R1>.CreateRight(rMap(r))
    );
  }

  public Either<L, R1> RFlatMap<R1>(Func<R, Either<L, R1>> rMap) {
    ArgumentNullException.ThrowIfNull(rMap);
    return either.Match(onLeft: Either<L, R1>.CreateLeft, onRight: rMap);
  }

  public Either<L1, R> LFlatMap<L1>(Func<L, Either<L1, R>> lMap) {
    ArgumentNullException.ThrowIfNull(lMap);
    return either.Match(onLeft: lMap, onRight: Either<L1, R>.CreateRight);
  }

  public Either<L1, R1> BFlatMap<L1, R1>(Func<L, Either<L1, R1>> lMap, Func<R, Either<L1, R1>> rMap) {
    ArgumentNullException.ThrowIfNull(lMap);
    ArgumentNullException.ThrowIfNull(rMap);
    return either.Match(onLeft: lMap, onRight: rMap);
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