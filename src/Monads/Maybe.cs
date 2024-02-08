namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly IMaybe maybe;

  public Maybe() {
    maybe = new None();
  }

  Maybe(IMaybe maybe) {
    this.maybe = maybe;
  }

  public TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> onSome) {
    ArgumentNullException.ThrowIfNull(onNone);
    ArgumentNullException.ThrowIfNull(onSome);
    return maybe.Match(onNone: onNone, onSome: onSome);
  }

  internal static Maybe<T> CreateNone() => new();
  internal static Maybe<T> CreateSome(T value) => new(new Some(value));

  interface IMaybe {
    TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> onSome);
  }

  readonly record struct Some : IMaybe {
    readonly T value;

    public Some(T value) {
      ArgumentNullException.ThrowIfNull(value);
      this.value = value;
    }

    public TResult Match<TResult>(Func<TResult> _, Func<T, TResult> onSome) => onSome(value);
  }

  readonly record struct None : IMaybe {
    public TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> _) => onNone();
  }
}