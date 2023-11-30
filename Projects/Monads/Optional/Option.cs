using System;

namespace Frognar.Monads.Optional;

public struct Option<T> where T : class {
  T? value;

  Option(T? value) {
    this.value = value;
  }

  public static Option<T> Some(T obj) => new(obj);
  public static Option<T> None => new(null);
  
  public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return value is null ? Option<TResult>.None : Option<TResult>.Some(map(value));
  }
}