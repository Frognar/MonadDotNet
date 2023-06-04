namespace Frognar.Monads; 

public readonly struct Result<T> {
  public T Value { get; }
  public bool IsSuccess { get; }
  
  Result(T value) {
    Value = value;
    IsSuccess = true;
  }

  public static Result<T> Ok(T value) => new(value);
}