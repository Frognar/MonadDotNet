using System;
using System.Collections.Generic;
using System.Linq;

namespace Frognar.Monads {
  public readonly struct Logged<T> {
    public T Value { get; }
    public IEnumerable<string> Logs { get; }
    
    public Logged(T value, IEnumerable<string> logs) {
      Value = value;
      Logs = logs;
    }
    
    public static Logged<T> Of(T value) => new Logged<T>(value, Array.Empty<string>());
    public static Logged<T> Of(T value, string log) => new Logged<T>(value, new[] { log });
    
    public static implicit operator Logged<T>(T value) => Of(value);
  }

  public static class LoggedExtensions {
    public static Logged<U> RunWithLogs<T, U>(this T value, Func<T, Logged<U>> func) =>
      Logged<T>.Of(value).RunWithLogs(func);
    
    public static Logged<U> RunWithLogs<T, U>(this Logged<T> value, Func<T, Logged<U>> func) {
      Logged<U> x = func(value.Value);
      return new Logged<U>(x.Value, value.Logs.Concat(x.Logs).ToArray());
    }
  }
}