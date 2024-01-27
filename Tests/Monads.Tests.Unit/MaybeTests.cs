using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class MaybeTests {
  static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
  static Maybe<T> None<T>() => Maybe<T>.None();

  [Fact]
  public void ReturnsInternalValueIfCreatedWithValue() {
    Some(10)
      .OrElse(-1)
      .Should().Be(10);
  }

  [Fact]
  public void ReturnsFallbackValueIfCreatedWithoutValue() {
    None<int>()
      .OrElse(-1)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowsExceptionWhenSomeIsCalledWithNull() {
    Action act = () => Some<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenOrElseIsCalledWithNull() {
    Func<string> act = () => Some("str").OrElse(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void FlatMapsValueWhenSome() {
    Some(10)
      .SelectMany(value => Some(value.ToString()))
      .OrElse("")
      .Should().Be("10");
  }

  [Fact]
  public void PropagetesFlattenedNoneWhenNone() {
    None<int>()
      .SelectMany(value => Some(value.ToString()))
      .OrElse("none")
      .Should().Be("none");
  }
}