using System.Collections.Generic;

namespace Frognar.Monads.Writers;

public readonly struct WriterResult<T, TLog> {
  public readonly T Value;
  public readonly IEnumerable<TLog> Logs;

  public WriterResult(T value, IEnumerable<TLog> logs) {
    Value = value;
    Logs = logs;
  }
}