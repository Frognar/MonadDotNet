namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly IMaybe maybe;

  public Maybe() {
    maybe = new None();
  }

  Maybe(IMaybe maybe) {
    this.maybe = maybe;
  }

  public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) {
    ArgumentNullException.ThrowIfNull(some);
    ArgumentNullException.ThrowIfNull(none);
    return maybe.Match(onSome: some, onNone: none);
  }

  internal static Maybe<T> CreateNone() => new();
  internal static Maybe<T> CreateSome(T value) => new(new Some(value));

  interface IMaybe {
    TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);
  }

  readonly record struct Some : IMaybe {
    readonly T value;

    public Some(T value) {
      ArgumentNullException.ThrowIfNull(value);
      this.value = value;
    }

    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> _) => onSome(value);
  }

  readonly record struct None : IMaybe {
    public TResult Match<TResult>(Func<T, TResult> _, Func<TResult> onNone) => onNone();
  }
}