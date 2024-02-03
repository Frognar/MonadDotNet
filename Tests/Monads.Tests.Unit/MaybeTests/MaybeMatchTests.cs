namespace Monads.Tests.Unit.MaybeTests;

public class MaybeMatchTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTimes2TestData))]
  public void MatchSomeWhenSome(int value, int expected) {
    Some(value)
      .Match(
        some: MultiplyBy2,
        none: Minus1)
      .Should().Be(expected);
  }

  [Fact]
  public void MatchNoneWhenNone() {
    None<int>()
      .Match(
        some: MultiplyBy2,
        none: Minus1)
      .Should().Be(-1);
  }

  [Fact]
  public void NoneMatchIsNotCalledWhenSome() {
    Func<int> act =
      () => Some(10)
        .Match(
          some: MultiplyBy2,
          none: ThrowUnreachable);

    act.Should().NotThrow();
  }

  [Fact]
  public void SomeMatchIsNotCalledWhenNone() {
    Func<int> act =
      () => None<int>()
        .Match(
          some: _ => ThrowUnreachable(),
          none: Minus1);

    act.Should().NotThrow();
  }

  [Fact]
  public void ThrowsExceptionWhenSomeMatchIsNull() {
    Func<int> act =
      () => None<int>()
        .Match(
          some: null!,
          none: Minus1);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenNoneMatchIsNull() {
    Func<int> act =
      () => Some(10)
        .Match(
          some: MultiplyBy2,
          none: null!);

    act.Should().Throw<ArgumentNullException>();
  }
}