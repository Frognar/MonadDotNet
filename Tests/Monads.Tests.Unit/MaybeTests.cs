using System.Diagnostics;
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
  public void PropagatesFlattenedNoneWhenNone() {
    None<int>()
      .SelectMany(value => Some(value.ToString()))
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void MapsValueWhenSome() {
    Some(10)
      .Select(value => value.ToString())
      .OrElse("")
      .Should().Be("10");
  }

  [Fact]
  public void PropagatesNoneWhenNone() {
    None<int>()
      .Select(value => value.ToString())
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void ReturnsNoneWhenSelectorReturnsNull() {
    Some(10)
      .Select(_ => (string?)null)
      .OrElse("none")
      .Should().Be("none");
  }

  [Fact]
  public void ThrowsExceptionWhenSelectorIsNull() {
    Action act = () => Some(10).Select<int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenFlatSelectorIsNull() {
    Action act = () => Some(10).SelectMany<int>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void FlattensNestedMaybe() {
    Some(Some(10))
      .Flatten()
      .OrElse(-1)
      .Should().Be(10);
  }

  [Fact]
  public void MapsValuesWithQuerySyntax() {
    Maybe<int> result = from a in Some(1)
      from b in Some(2)
      from c in Some(3)
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(6);
  }

  [Fact]
  public void PropagatesNoneWithQuerySyntax() {
    Maybe<int> result = from a in Some(1)
      from b in Some(2)
      from c in None<int>()
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(-1);
  }

  [Fact]
  public void MatchSomeWhenSome() {
    Some(10)
      .Match(
        some: value => value * 2,
        none: () => -1)
      .Should().Be(20);
  }

  [Fact]
  public void MatchNoneWhenNone() {
    None<int>()
      .Match(
        some: value => value,
        none: () => -1)
      .Should().Be(-1);
  }

  [Fact]
  public void NoneMatchIsNotCalledWhenSome() {
    Func<int> act =
      () => Some(10)
        .Match(
          some: value => value * 2,
          none: () => throw new UnreachableException());
    
    act.Should().NotThrow();
  }

  [Fact]
  public void SomeMatchIsNotCalledWhenNone() {
    Func<int> act =
      () => None<int>()
        .Match(
          some: _ => throw new UnreachableException(),
          none: () => -1);
    
    act.Should().NotThrow();
  }
}