using System.Collections.Generic;

namespace Frognar.Monads.Writers;

public readonly struct Writer<T, TLog> {
  public T Value { get; }
  public IEnumerable<TLog> Logs { get; }

  public Writer(T value, IEnumerable<TLog> logs) {
    Value = value;
    Logs = logs;
  }
}