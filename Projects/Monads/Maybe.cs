namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly T value;

  public Maybe(T value) {
    this.value = value;
  }

  public T OrElse(T defaultValue) {
    return value;
  }
}