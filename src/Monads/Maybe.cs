namespace Frognar.Monads;

public interface IMaybe<out T> {
  TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> onSome);
}

public static class Maybe {
  public static IMaybe<T> None<T>() => new NoneMaybe<T>();
  public static IMaybe<T> Some<T>(T value) => new SomeMaybe<T>(value);

  readonly record struct SomeMaybe<T> : IMaybe<T> {
    readonly T value;

    public SomeMaybe(T value) {
      ArgumentNullException.ThrowIfNull(value);
      this.value = value;
    }

    public TResult Match<TResult>(Func<TResult> _, Func<T, TResult> onSome) {
      ArgumentNullException.ThrowIfNull(_);
      ArgumentNullException.ThrowIfNull(onSome);
      return onSome(value);
    }
  }

  readonly record struct NoneMaybe<T> : IMaybe<T> {
    public TResult Match<TResult>(Func<TResult> onNone, Func<T, TResult> _) {
      ArgumentNullException.ThrowIfNull(onNone);
      ArgumentNullException.ThrowIfNull(_);
      return onNone();
    }
  }
}