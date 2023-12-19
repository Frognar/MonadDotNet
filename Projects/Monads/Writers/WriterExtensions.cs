using System.Collections.Generic;
using System.Collections.Immutable;

namespace Frognar.Monads.Writers;

public static class WriterExtensions {
  public static Writer<T, TLog> WithLogs<T, TLog>(this T value, IEnumerable<TLog> logs) {
    return Writer<T, TLog>.Wrap(value, logs.ToImmutableList());
  }
}