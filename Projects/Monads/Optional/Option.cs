using System;

namespace Frognar.Monads.Optional;

public struct Option<T> : IEquatable<Option<T>> where T : class {
  T? value;

  Option(T? value) {
    this.value = value;
  }

  public static Option<T> Some(T obj) => new(obj);
  public static Option<T> None => new(null);
  
  public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return value is null ? Option<TResult>.None : Option<TResult>.Some(map(value));
  }
  
  public Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map) where TResult : class {
    return value is null ? Option<TResult>.None : map(value);
  }
  
  public T Reduce(T defaultValue) {
    return value ?? defaultValue;
  }
  
  public T Reduce(Func<T> defaultValue) {
    return value ?? defaultValue();
  }
  
  public Option<T> Where(Func<T, bool> predicate) {
    return value is not null && predicate(value) ? this : None;
  }
  
  public Option<T> WhereNot(Func<T, bool> predicate) {
    return value is not null && !predicate(value) ? this : None;
  }

  public override int GetHashCode() {
    return value?.GetHashCode() ?? 0;
  }

  public bool Equals(Option<T> other) {
    return value?.Equals(other.value) ?? other.value is null;
  }

  public override bool Equals(object? obj) {
    return obj is Option<T> other && Equals(other);
  }
  
  public static bool operator ==(Option<T>? a, Option<T>? b) => a is null ? b is null : a.Equals(b);
  public static bool operator !=(Option<T>? a, Option<T>? b) => !(a == b);
}