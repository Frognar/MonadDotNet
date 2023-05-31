namespace Frognar.Monads; 

public readonly struct Try<T> {
  public T Value => (T)(object)5;
  public bool IsSuccess => true;
  
  public static Try<T> Success(T value) {
    return new Try<T>();
  }
}