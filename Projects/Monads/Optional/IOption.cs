using System;
using System.Threading.Tasks;

namespace Frognar.Monads.Optional;

public interface IOption<T> {
  IOption<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class;
  Task<IOption<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map) where TResult : class;
  IOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct;
  Task<IOption<TResult>> MapValueAsync<TResult>(Func<T, Task<TResult>> map) where TResult : struct;
  IOption<TResult> FlatMap<TResult>(Func<T, IOption<TResult>> map) where TResult : class;
  Task<IOption<TResult>> FlatMapAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : class;
  IOption<TResult> FlatMapValue<TResult>(Func<T, IOption<TResult>> map) where TResult : struct;
  Task<IOption<TResult>> FlatMapValueAsync<TResult>(Func<T, Task<IOption<TResult>>> map) where TResult : struct;
  T OrElse(T defaultValue);
  T OrElseGet(Func<T> defaultValue);
  Task<T> OrElseGetAsync(Func<Task<T>> defaultValue);
  IOption<T> Where(Func<T, bool> predicate);
  Task<IOption<T>> WhereAsync(Func<T, Task<bool>> predicate);
  IOption<T> WhereNot(Func<T, bool> predicate);
  Task<IOption<T>> WhereNotAsync(Func<T, Task<bool>> predicate);
  void IfPresent(Action<T> action);
  Task IfPresentAsync(Func<T, Task> action);
  bool IsPresent();
  void Switch(Action<T> onValue, Action onNone);
}