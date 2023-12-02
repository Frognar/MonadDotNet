using System;

namespace Frognar.Monads.Optional;

public static class OptionalExtensions
{
    public static IOption<T> ToOption<T>(this T? obj) where T : class {
        return obj is null ? Option<T>.None() : Option<T>.Some(obj);
    }

    public static IOption<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : class {
        return obj is not null && predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();
    }

    public static IOption<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : class {
        return obj is not null && !predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();
    }
    
    public static IOption<T> ToOption<T>(this T? obj) where T : struct {
        return obj.HasValue == false ? ValueOption<T>.None() : ValueOption<T>.Some(obj.Value);
    }

    public static IOption<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : struct {
        return obj.HasValue && predicate(obj.Value) ? ValueOption<T>.Some(obj.Value) : ValueOption<T>.None();
    }

    public static IOption<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : struct {
        return obj.HasValue && !predicate(obj.Value) ? ValueOption<T>.Some(obj.Value) : ValueOption<T>.None();
    }
}