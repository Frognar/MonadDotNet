namespace Frognar.Monads;

public readonly record struct Maybe<T> {
  readonly T value;
  readonly bool hasValue;

  public Maybe(T value) {
    this.value = value;
    hasValue = true;
  }

  public T OrElse(T defaultValue) {
    return hasValue ? value : defaultValue;
  }
}