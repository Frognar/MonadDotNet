using System;
using System.Threading.Tasks;

namespace Frognar.Monads.Optional;

public readonly record struct ValueOption<T> : IOption<T> where T : struct {
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

  public T OrElse(T defaultValue) {
    return value ?? defaultValue;
  }

  public T OrElseThrow(Func<Exception> exception) {
    return value ?? throw exception();
  }

  public async Task<T> OrElseGetAsync(Func<Task<T>> defaultValue) {
    return value ?? await defaultValue();
  }

  public T OrElseGet(Func<T> defaultValue) {
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
  
  public async Task SwitchAsync(Func<T, Task> onValue, Func<Task> onNone) {
    if (value.HasValue) {
      await onValue(value.Value);
    }
    else {
      await onNone();
    }
  }
}