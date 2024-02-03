using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class ThreeIntSumTestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue, -1, 0, int.MaxValue]; // underflow
    yield return [int.MinValue, 0, 0, int.MinValue];
    yield return [-15, -30, 60, 15];
    yield return [-10, -20, -30, -60];
    yield return [0, 0, 0, 0];
    yield return [10, 20, 30, 60];
    yield return [15, 30, -60, -15];
    yield return [int.MaxValue, 0, 0, int.MaxValue];
    yield return [int.MaxValue, 1, 0, int.MinValue]; // overflow
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}