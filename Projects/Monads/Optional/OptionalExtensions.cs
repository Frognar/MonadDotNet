using System;

namespace Frognar.Monads.Optional;

public static class OptionalExtensions {
  public static Option<T> ToOption<T>(this T? obj) {
    return Option<T>.SomeNullable(obj);
  }

  public static Option<T> Where<T>(this T? obj, Func<T, bool> predicate) {
    return obj.ToOption().Where(predicate);
  }

  public static Option<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) {
    return obj.ToOption().WhereNot(predicate);
  }
}