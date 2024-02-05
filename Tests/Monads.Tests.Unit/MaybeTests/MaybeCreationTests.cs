using Monads.Tests.Unit.MaybeTests.Helpers.TestDataGenerators;

namespace Monads.Tests.Unit.MaybeTests;

public class MaybeCreationTests {
  [Theory]
  [ClassData(typeof(IntTestData))]
  public void ReturnsMaybeWithSome(int value) {
    Some(value)
      .Should().Be(Maybe.Some(value));
  }

  [Fact]
  public void ReturnsMaybeWithNone() {
    None<int>()
      .Should().Be(new Maybe<int>());
  }

  [Fact]
  public void ThrowsExceptionWhenSomeNull() {
    Action act = () => Some<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }
}