using System;

namespace Frognar.Monads.Optional;

public interface IOption<T> {
  IOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class;
  IOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct;
  IOption<TResult> FlatMap<TResult>(Func<T, IOption<TResult>> map) where TResult : class;
  IOption<TResult> FlatMapValue<TResult>(Func<T, IOption<TResult>> map) where TResult : struct;
  T Reduce(T defaultValue);
  T Reduce(Func<T> defaultValue);
  IOption<T> Where(Func<T, bool> predicate);
  IOption<T> WhereNot(Func<T, bool> predicate);
}