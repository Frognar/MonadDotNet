using System.Collections;

namespace Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

public class StringTestData : IEnumerable<object[]> {
  public IEnumerator<object[]> GetEnumerator() {
    yield return [""];
    yield return ["test"];
    yield return ["query syntax"];
    yield return ["null is bad"];
    yield return ["123"];
    yield return ["-536323"];
    yield return [int.MaxValue.ToString()];
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}