using System;

namespace Frognar.Monads.Optional;

public readonly struct Option<T> : IOption<T>, IEquatable<Option<T>> where T : class {
  readonly T? value;

  Option(T? value) {
    this.value = value;
  }

  public static IOption<T> Some(T obj) {
    return new Option<T>(obj);
  }

  public static IOption<T> None() {
    return new Option<T>(null);
  }

  public IOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return value is null ? Option<TResult>.None() : Option<TResult>.Some(map(value));
  }

  public IOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : ValueOption<TResult>.Some(map(value));
  }

  public IOption<TResult> FlatMap<TResult>(Func<T, IOption<TResult>> map) where TResult : class {
    return value is null ? Option<TResult>.None() : map(value);
  }

  public IOption<TResult> FlatMapValue<TResult>(Func<T, IOption<TResult>> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : map(value);
  }

  public T Reduce(T defaultValue) {
    return value ?? defaultValue;
  }

  public T Reduce(Func<T> defaultValue) {
    return value ?? defaultValue();
  }

  public IOption<T> Where(Func<T, bool> predicate) {
    return value is not null && predicate(value) ? this : None();
  }

  public IOption<T> WhereNot(Func<T, bool> predicate) {
    return value is not null && !predicate(value) ? this : None();
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

  public static bool operator ==(Option<T>? a, Option<T>? b) {
    return a?.Equals(b) ?? b is null;
  }

  public static bool operator !=(Option<T>? a, Option<T>? b) {
    return !(a == b);
  }
}