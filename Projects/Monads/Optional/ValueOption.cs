﻿using System;
using System.Threading.Tasks;

namespace Frognar.Monads.Optional;

public readonly struct ValueOption<T> : IOption<T>, IEquatable<ValueOption<T>> where T : struct {
  readonly T? value;

  ValueOption(T? value) {
    this.value = value;
  }

  public static IOption<T> Some(T obj) => new ValueOption<T>(obj);
  public static IOption<T> SomeNullable(T? obj) => new ValueOption<T>(obj);
  public static IOption<T> None() => new ValueOption<T>(null);

  public IOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return value.HasValue == false ? Option<TResult>.None() : Option<TResult>.SomeNullable(map(value.Value));
  }
  
  public async Task<IOption<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) where TResult : class {
    return value.HasValue == false ? Option<TResult>.None() : Option<TResult>.SomeNullable(await map(value.Value).ConfigureAwait(false));
  }

  public IOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct {
    return value.HasValue == false ? ValueOption<TResult>.None() : ValueOption<TResult>.SomeNullable(map(value.Value));
  }
  
  public async Task<IOption<TResult>> MapValueAsync<TResult>(Func<T, Task<TResult>> map) where TResult : struct {
    return value.HasValue == false ? ValueOption<TResult>.None() : ValueOption<TResult>.SomeNullable(await map(value.Value).ConfigureAwait(false));
  }

  public IOption<TResult> FlatMap<TResult>(Func<T, IOption<TResult>> map) where TResult : class {
    return value.HasValue == false ? Option<TResult>.None() : map(value.Value);
  }
  
  public async Task<IOption<TResult>> FlatMapAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : class {
    return value.HasValue == false ? Option<TResult>.None() : await map(value.Value).ConfigureAwait(false);
  }

  public IOption<TResult> FlatMapValue<TResult>(Func<T, IOption<TResult>> map) where TResult : struct {
    return value.HasValue == false ? ValueOption<TResult>.None() : map(value.Value);
  }

  public async Task<IOption<TResult>> FlatMapValueAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : struct {
    return value.HasValue == false ? ValueOption<TResult>.None() : await map(value.Value).ConfigureAwait(false);
  }

  public T Reduce(T defaultValue) {
    return value ?? defaultValue;
  }

  public async Task<T> ReduceAsync(Func<Task<T>> defaultValue) {
    return value ?? await defaultValue();
  }

  public T Reduce(Func<T> defaultValue) {
    return value ?? defaultValue();
  }

  public IOption<T> Where(Func<T, bool> predicate) {
    return value.HasValue && predicate(value.Value) ? this : None();
  }

  public async Task<IOption<T>> WhereAsync(Func<T, Task<bool>> predicate) {
    return value.HasValue && await predicate(value.Value) ? this : None();
  }

  public IOption<T> WhereNot(Func<T, bool> predicate) {
    return value.HasValue && predicate(value.Value) == false ? this : None();
  }

  public async Task<IOption<T>> WhereNotAsync(Func<T, Task<bool>> predicate) {
    return value.HasValue && await predicate(value.Value) == false ? this : None();
  }

  public void IfPresent(Action<T> action) {
    if (value.HasValue) {
      action(value.Value);
    }
  }

  public async Task IfPresentAsync(Func<T, Task> action) {
    if (value.HasValue) {
      await action(value.Value);
    }
  }

  public bool IsPresent() {
    return value.HasValue;
  }

  public void Switch(Action<T> onValue, Action onNone) {
    if (value.HasValue) {
      onValue(value.Value);
    }
    else {
      onNone();
    }
  }

  public override int GetHashCode() {
    return value.GetHashCode();
  }

  public bool Equals(ValueOption<T> other) {
    return Nullable.Equals(value, other.value);
  }

  public override bool Equals(object? obj) {
    return obj is ValueOption<T> other && Equals(other);
  }

  public static bool operator ==(ValueOption<T> a, ValueOption<T> b) => a.Equals(b);
  public static bool operator !=(ValueOption<T> a, ValueOption<T> b) => !(a == b);
}