using System;

namespace Frognar.Monads {
  public readonly struct Maybe<T> {
    public T Value { get; }
    public bool HasValue => Value != null;
    
    public Maybe(T value) => Value = value;
    
    public static Maybe<T> None { get; } = new Maybe<T>();
    public static Maybe<T> Of(T value) => new Maybe<T>(value);

    public static implicit operator Maybe<T>(T value) => Of(value);
  }

  public static class MaybeExtensions {
    public static Maybe<U> Run<T, U>(this T value, Func<T, Maybe<U>> func) => Maybe<T>.Of(value).Run(func);
    public static Maybe<U> Run<T, U>(this Maybe<T> value, Func<T, Maybe<U>> func) => value.HasValue ? func(value.Value) : Maybe<U>.None;
  }
}