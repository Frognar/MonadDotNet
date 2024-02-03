using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class IntFilteredBeLessThanOrEqualTo0TestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue, int.MinValue];
    yield return [-15, -15];
    yield return [-10, -10];
    yield return [0, 0];
    yield return [10, -1];
    yield return [15, -1];
    yield return [int.MaxValue, -1];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}