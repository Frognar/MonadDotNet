using System.Collections.Immutable;

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
}