using System;
using System.Threading.Tasks;

namespace Frognar.Monads.Optional;

public readonly record struct Option<T> : IOption<T> where T : class {
  readonly T? value;

  Option(T? value) {
    this.value = value;
  }

  public static IOption<T> Some(T obj) => new Option<T>(obj);
  public static IOption<T> SomeNullable(T? obj) => new Option<T>(obj);
  public static IOption<T> None() => new Option<T>(null);

  public IOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class {
    return value is null ? Option<TResult>.None() : Option<TResult>.SomeNullable(map(value));
  }
  
  public async Task<IOption<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) where TResult : class {
    return value is null ? Option<TResult>.None() : Option<TResult>.SomeNullable(await map(value).ConfigureAwait(false));
  }

  public IOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : ValueOption<TResult>.SomeNullable(map(value));
  }
  
  public async Task<IOption<TResult>> MapValueAsync<TResult>(Func<T, Task<TResult>> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : ValueOption<TResult>.SomeNullable(await map(value).ConfigureAwait(false));
  }

  public IOption<TResult> FlatMap<TResult>(Func<T, IOption<TResult>> map) where TResult : class {
    return value is null ? Option<TResult>.None() : map(value);
  }
  
  public async Task<IOption<TResult>> FlatMapAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : class {
    return value is null ? Option<TResult>.None() : await map(value).ConfigureAwait(false);
  }

  public IOption<TResult> FlatMapValue<TResult>(Func<T, IOption<TResult>> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : map(value);
  }

  public async Task<IOption<TResult>> FlatMapValueAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : struct {
    return value is null ? ValueOption<TResult>.None() : await map(value).ConfigureAwait(false);
  }

  public T OrElse(T defaultValue) {
    return value ?? defaultValue;
  }

  public T OrElseGet(Func<T> defaultValue) {
    return value ?? defaultValue();
  }

  public T OrElseThrow(Func<Exception> exception) {
    return value ?? throw exception();
  }

  public async Task<T> OrElseGetAsync(Func<Task<T>> defaultValue) {
    return value ?? await defaultValue();
  }

  public IOption<T> Where(Func<T, bool> predicate) {
    return value is not null && predicate(value) ? this : None();
  }

  public async Task<IOption<T>> WhereAsync(Func<T, Task<bool>> predicate) {
    return value is not null && await predicate(value) ? this : None();
  }

  public IOption<T> WhereNot(Func<T, bool> predicate) {
    return value is not null && predicate(value) == false ? this : None();
  }

  public async Task<IOption<T>> WhereNotAsync(Func<T, Task<bool>> predicate) {
    return value is not null && await predicate(value) == false ? this : None();
  }

  public void IfPresent(Action<T> action) {
    if (value is not null) {
      action(value);
    }
  }

  public async Task IfPresentAsync(Func<T, Task> action) {
    if (value is not null) {
      await action(value);
    }
  }

  public bool IsPresent() {
    return value is not null;
  }

  public void Switch(Action<T> onValue, Action onNone) {
    if (value is not null) {
      onValue(value);
    }
    else {
      onNone();
    }
  }

  public async Task SwitchAsync(Func<T, Task> onValue, Func<Task> onNone) {
    if (value is not null) {
      await onValue(value);
    }
    else {
      await onNone();
    }
  }
}