using System;

namespace Frognar.Monads.Optional;

public static class OptionalExtensions {
  public static IOption<T> ToOption<T>(this T? obj) where T : class {
    return Option<T>.SomeNullable(obj);
  }

  public static IOption<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : class {
    return obj.ToOption().Where(predicate);
  }

  public static IOption<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : class {
    return obj.ToOption().WhereNot(predicate);
  }

  public static IOption<T> ToOption<T>(this T? obj) where T : struct {
    return ValueOption<T>.SomeNullable(obj);
  }

  public static IOption<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : struct {
    return obj.ToOption().Where(predicate);
  }

  public static IOption<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : struct {
    return obj.ToOption().WhereNot(predicate);
  }
}