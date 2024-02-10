namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly IMaybe maybe;

  public Maybe() : this(new None()) {
  }

  Maybe(IMaybe maybe) {
    this.maybe = maybe;
  }

  public TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> onSome) {
    ArgumentNullException.ThrowIfNull(onNone);
    ArgumentNullException.ThrowIfNull(onSome);
    return maybe.Match(onNone: onNone, onSome: onSome);
  }

  public Maybe<TResult> Map<TResult>(Func<T, TResult> map) {
    ArgumentNullException.ThrowIfNull(map);
    return maybe.Match(
      onNone: Maybe<TResult>.CreateNone,
      onSome: value => Maybe<TResult>.CreateSome(map(value))
    );
  }

  public Maybe<TResult> FlatMap<TResult>(Func<T, Maybe<TResult>> map) {
    ArgumentNullException.ThrowIfNull(map);
    return maybe.Match(
      onNone: Maybe<TResult>.CreateNone,
      onSome: map
    );
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