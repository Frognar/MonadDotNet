using System;

namespace Frognar.Monads {
  public readonly struct Option<T> : Monad<T> {
    public T Value { get; }
    public bool HasValue { get; }
    
    Option(T value) {
      Value = value;
      HasValue = true;
    }
    
    public static Option<T> Some(T value) => new Option<T>(value);
    public static Option<T> None { get; } = new Option<T>();
    
    public Monad<U> Bind<U>(Func<T, Monad<U>> func) {
      return HasValue ? func(Value) : Option<U>.None;
    }
    
    public static implicit operator Option<T>(T value) => Some(value);
  }
}