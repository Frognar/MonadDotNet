using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class IntPlusOneTestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue, int.MinValue + 1];
    yield return [-15, -14];
    yield return [-10, -9];
    yield return [0, 1];
    yield return [10, 11];
    yield return [15, 16];
    yield return [int.MaxValue, int.MinValue];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}