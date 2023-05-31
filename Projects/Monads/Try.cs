namespace Frognar.Monads; 

public readonly struct Try<T> {
  readonly T value;

  public T Value => value;
  public bool IsSuccess { get; }

  Try(T value) {
    this.value = value;
    IsSuccess = true;
  }
  
  public static Try<T> Success(T value) {
    return new Try<T>(value);
  }
}