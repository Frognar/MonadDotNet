using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class IntToStringTestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [int.MinValue, int.MinValue.ToString()];
    yield return [-15, "-15"];
    yield return [-10, "-10"];
    yield return [0, "0"];
    yield return [10, "10"];
    yield return [15, "15"];
    yield return [int.MaxValue, int.MaxValue.ToString()];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}