using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class IntTestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue];
    yield return [-15];
    yield return [-10];
    yield return [0];
    yield return [10];
    yield return [15];
    yield return [int.MaxValue];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}