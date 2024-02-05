using System.Diagnostics;

namespace Monads.Tests.Unit.MaybeTests.Helpers;

public static class MaybeHelperMethods {
  public static Maybe<T> Some<T>(T value) => Maybe.Some(value);
  public static Maybe<T> None<T>() => Maybe.None<T>();
  public static int MultiplyBy2(int x) => x * 2;
  public static int Minus1() => -1;
  public static int ThrowUnreachable() => throw new UnreachableException();
  public static Maybe<string> ToMaybeString(int x) => Some(x.ToString());
  public static string IntToString(int x) => x.ToString();
  public static string Id(string x) => x;
  public static int GetLength(string x) => x.Length;
  public static bool IsEven(int x) => x % 2 == 0;
  public static Maybe<int> SaveDiv1By(int x) => x == 0 ? None<int>() : Some(1 / x);

  public static Maybe<int> TryParseInt(string value) =>
    int.TryParse(s: value, result: out int result) ? Some(result) : None<int>();
}