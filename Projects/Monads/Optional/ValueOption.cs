using System;

namespace Frognar.Monads.Optional;

public readonly struct ValueOption<T> : IEquatable<ValueOption<T>> where T : struct
{
    private readonly T? value;

    ValueOption(T? value)
    {
        this.value = value;
    }

    public static ValueOption<T> Some(T obj) => new(obj);
    public static ValueOption<T> None() => new(null);
  
    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
        return value.HasValue == false ? Option<TResult>.None() : Option<TResult>.Some(map(value.Value));
    }
  
    public ValueOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct {
        return value.HasValue == false ? ValueOption<TResult>.None() : ValueOption<TResult>.Some(map(value.Value));
    }
  
    public Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map) where TResult : class {
        return value.HasValue == false ? Option<TResult>.None() : map(value.Value);
    }
  
    public ValueOption<TResult> FlatMapValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct {
        return value.HasValue == false ? ValueOption<TResult>.None() : map(value.Value);
    }
  
    public T Reduce(T defaultValue) {
        return value ?? defaultValue;
    }
  
    public T Reduce(Func<T> defaultValue) {
        return value ?? defaultValue();
    }
  
    public ValueOption<T> Where(Func<T, bool> predicate) {
        return value.HasValue && predicate(value.Value) ? this : None();
    }
  
    public ValueOption<T> WhereNot(Func<T, bool> predicate) {
        return value.HasValue && !predicate(value.Value) ? this : None();
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public bool Equals(ValueOption<T> other)
    {
        return Nullable.Equals(value, other.value);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueOption<T> other && Equals(other);
    }
  
    public static bool operator ==(ValueOption<T> a, ValueOption<T> b) => a.Equals(b);
    public static bool operator !=(ValueOption<T> a, ValueOption<T> b) => !(a == b);
}