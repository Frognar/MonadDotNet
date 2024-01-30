namespace Monads.Tests.Unit.MaybeTests;

public class MaybeCreationTests {
  [Theory]
  [InlineData(0)]
  [InlineData(10)]
  [InlineData(-15)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void ReturnsMaybeWithSome(int value) {
    Some(value)
      .Should().Be(new Maybe<int>(value));
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