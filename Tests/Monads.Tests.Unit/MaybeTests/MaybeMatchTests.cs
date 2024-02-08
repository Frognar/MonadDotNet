namespace Monads.Tests.Unit.MaybeTests;

public class MaybeMatchTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTimes2TestData))]
  public void MatchSomeWhenSome(int value, int expected) {
    Some(value)
      .Match(
        onSome: MultiplyBy2,
        onNone: Minus1)
      .Should().Be(expected);
  }

  [Fact]
  public void MatchNoneWhenNone() {
    None<int>()
      .Match(
        onSome: MultiplyBy2,
        onNone: Minus1)
      .Should().Be(-1);
  }

  [Fact]
  public void NoneMatchIsNotCalledWhenSome() {
    Func<int> act =
      () => Some(10)
        .Match(
          onSome: MultiplyBy2,
          onNone: ThrowUnreachable);

    act.Should().NotThrow();
  }

  [Fact]
  public void SomeMatchIsNotCalledWhenNone() {
    Func<int> act =
      () => None<int>()
        .Match(
          onSome: _ => ThrowUnreachable(),
          onNone: Minus1);

    act.Should().NotThrow();
  }

  [Fact]
  public void ThrowsExceptionWhenSomeMatchIsNull() {
    Func<int> act =
      () => None<int>()
        .Match(
          onSome: null!,
          onNone: Minus1);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenNoneMatchIsNull() {
    Func<int> act =
      () => Some(10)
        .Match(
          onSome: MultiplyBy2,
          onNone: null!);

    act.Should().Throw<ArgumentNullException>();
  }
}