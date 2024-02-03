namespace Frognar.Monads.Optional;

public readonly record struct Option<T> {
  readonly T value;

  Option(T value) {
    this.value = value;
  }

  public static Option<T> Some(T obj) => new(obj);
  public static Option<T> SomeNullable(T? obj) => obj is null ? None() : Some(obj);
  public static Option<T> None() => new();

  public Option<TResult> Map<TResult>(Func<T, TResult> map) {
    if (value is null) {
      return Option<TResult>.None();
    }

    TResult result = map(value);
    return Option<TResult>.SomeNullable(result);
  }

  public async Task<Option<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) {
    if (value is null) {
      return Option<TResult>.None();
    }

    TResult result = await map(value).ConfigureAwait(false);
    return Option<TResult>.SomeNullable(result);
  }

  public Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> map) {
    return value is null
      ? Option<TResult>.None()
      : map(value);
  }

  public async Task<Option<TResult>> FlatMapAsync<TResult>(Func<T, Task<Option<TResult>>> map) {
    return value is null
      ? Option<TResult>.None()
      : await map(value).ConfigureAwait(false);
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

  public Option<T> Where(Func<T, bool> predicate) {
    return value is not null && predicate(value)
      ? this
      : None();
  }

  public async Task<Option<T>> WhereAsync(Func<T, Task<bool>> predicate) {
    return value is not null && await predicate(value)
      ? this
      : None();
  }

  public Option<T> WhereNot(Func<T, bool> predicate) {
    return value is not null && predicate(value) == false
      ? this
      : None();
  }

  public async Task<Option<T>> WhereNotAsync(Func<T, Task<bool>> predicate) {
    return value is not null && await predicate(value) == false
      ? this
      : None();
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