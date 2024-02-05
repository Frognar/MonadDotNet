using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class IntTimes2TestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue, 0];
    yield return [-15, -30];
    yield return [-10, -20];
    yield return [0, 0];
    yield return [10, 20];
    yield return [15, 30];
    yield return [int.MaxValue, -2];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}