namespace Monads.Tests.Unit.MaybeTests.Helpers;

public static class MaybeHelperMethods {
  public static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
  public static Maybe<T> None<T>() => Maybe<T>.None();
}