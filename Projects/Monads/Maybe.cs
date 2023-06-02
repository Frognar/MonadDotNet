namespace Frognar.Monads; 

public readonly struct Maybe<T> {
  public bool HasValue => true;
  public int Value => 5;
  
  Maybe(int value) {
  }

  public static Maybe<int> From(int value) {
    return new Maybe<int>(value);
  }
}