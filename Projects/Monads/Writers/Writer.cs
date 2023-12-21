using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Frognar.Monads.Writers;

public readonly struct Writer<T, TLog> {
  public T Value { get; }
  public ImmutableList<TLog> Logs { get; }

  public Writer(T value, ImmutableList<TLog> logs) {
    Value = value;
    Logs = logs;
  }
  
  public static Writer<T, TLog> Wrap(T value, ImmutableList<TLog> logs) => new(value, logs);
  public static Writer<T, TLog> Wrap(T value, TLog log) => new(value, ImmutableList.Create(log));
  public static Writer<T, TLog> Wrap(T value) => new(value, ImmutableList<TLog>.Empty);
  
  public Writer<TResult, TLog> Map<TResult>(Func<T, TResult> map) {
    return new Writer<TResult, TLog>(map(Value), Logs);
  }
  
  public async Task<Writer<TResult, TLog>> MapAsync<TResult>(Func<T, Task<TResult>> map) {
    return new Writer<TResult, TLog>(await map(Value), Logs);
  }
  
  public Writer<TResult, TLog> FlatMap<TResult>(Func<T, Writer<TResult, TLog>> map) {
    Writer<TResult, TLog> result = map(Value);
    return new Writer<TResult, TLog>(result.Value, Logs.AddRange(result.Logs));
  }
  
  public async Task<Writer<TResult, TLog>> FlatMapAsync<TResult>(Func<T, Task<Writer<TResult, TLog>>> map) {
    Writer<TResult, TLog> result = await map(Value);
    return new Writer<TResult, TLog>(result.Value, Logs.AddRange(result.Logs));
  }
  
  public void ForEachLog(Action<TLog> action) {
    Logs.ForEach(action);
  }
  
  public async Task ForEachLogAsync(Func<TLog, Task> action) {
    foreach (TLog log in Logs) {
      await action(log);
    }
  }
}