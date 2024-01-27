using Frognar.Monads;

namespace Monads.Tests.Unit;

public class MaybeTests {
  [Fact]
  public void CreatingMaybeWithExistingValue() {
    Maybe<int> maybe = new(10);
  }
}