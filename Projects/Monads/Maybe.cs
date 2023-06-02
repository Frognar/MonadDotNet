namespace Frognar.Monads; 

public readonly struct Maybe<T> {
  public bool HasValue { get; }
  public T Value { get; }

  Maybe(T value) {
    Value = value;
    HasValue = value is not null;
  }

  public static Maybe<T> From(T value) {
    return new Maybe<T>(value);
  }
}