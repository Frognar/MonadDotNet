using System.Diagnostics;
using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class MaybeTests {
  [Fact]
  public void ThrowsExceptionWhenSomeNull() {
    Action act = () => Some<string>(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ReturnsInternalValueWhenSome() {
    Some(10)
      .OrElse(-1)
      .Should().Be(10);
  }

  [Fact]
  public void ReturnsFallbackValueWhenNone() {
    None<int>()
      .OrElse(-1)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowsExceptionWhenOrElseIsCalledWithNull() {
    Func<string> act = () => Some("str").OrElse((string)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void DefaultFactoryIsNotCalledWhenSome() {
    Some(10)
      .OrElse(() => throw new UnreachableException())
      .Should().Be(10);
  }

  [Fact]
  public void ReturnsFactoredFallbackValueWhenNone() {
    None<int>()
      .OrElse(() => -1)
      .Should().Be(-1);
  }

  [Fact]
  public void ThrowsExceptionWhenDefaultFactoryIsNull() {
    Func<string> act = () => Some("str").OrElse((Func<string>)null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapsValueWhenSome() {
    Some(10)
      .Select(ToString)
      .OrElse("")
      .Should().Be("10");
  }

  [Fact]
  public void PropagatesNoneWhenNone() {
    None<int>()
      .Select(ToString)
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
  public void FlatMapsValueWhenSome() {
    Some(10)
      .SelectMany(ToMaybeString)
      .OrElse("")
      .Should().Be("10");
  }

  [Fact]
  public void PropagatesFlattenedNoneWhenNone() {
    None<int>()
      .SelectMany(ToMaybeString)
      .OrElse("none")
      .Should().Be("none");
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

  [Fact]
  public void ThrowsExceptionWhenSomeMatchIsNull() {
    Func<int> act =
      () => None<int>()
        .Match(
          some: null!,
          none: () => -1);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenNoneMatchIsNull() {
    Func<int> act =
      () => Some(10)
        .Match(
          some: value => value,
          none: null!);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void MapsValuesWithSelectQuerySyntaxWhenSome() {
    Maybe<int> result =
      from a in Some(1)
      select a + 1;

    result.OrElse(-1)
      .Should().Be(2);
  }

  [Fact]
  public void PropagatesNoneWithSelectQuerySyntaxWhenNone() {
    Maybe<int> result =
      from a in None<int>()
      select a + 1;

    result.OrElse(-1)
      .Should().Be(-1);
  }

  [Fact]
  public void MapsValuesWithSelectManyQuerySyntaxWhenSome() {
    Maybe<int> result =
      from a in Some(1)
      from b in Some(2)
      from c in Some(3)
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(6);
  }

  [Fact]
  public void PropagatesNoneWithSelectManyQuerySyntaxWhenNone() {
    Maybe<int> result =
      from a in Some(1)
      from b in Some(2)
      from c in None<int>()
      select a + b + c;

    result.OrElse(-1)
      .Should().Be(-1);
  }

  [Theory]
  [InlineData("")]
  [InlineData("foo")]
  [InlineData("bar")]
  [InlineData("corge")]
  [InlineData("antidisestablishmentarianism")]
  public void ObeysFirstFunctorLawWhenSome(string value) {
    Some(value)
      .Should().Be(Some(value).Select(Id));
  }

  [Fact]
  public void ObeysFirstFunctorLawWhenNone() {
    None<string>()
      .Should().Be(None<string>().Select(Id));
  }

  [Theory]
  [InlineData("")]
  [InlineData("foo")]
  [InlineData("bar")]
  [InlineData("corge")]
  [InlineData("antidisestablishmentarianism")]
  public void ObeysSecondFunctorLawWhenSome(string value) {
    Some(value).Select(GetLength).Select(IsEven)
      .Should().Be(Some(value).Select(x => IsEven(GetLength(x))));
  }

  [Fact]
  public void ObeysSecondFunctorLawWhenNone() {
    None<string>().Select(GetLength).Select(IsEven)
      .Should().Be(None<string>().Select(x => IsEven(GetLength(x))));
  }

  [Theory]
  [InlineData(0)]
  [InlineData(1)]
  [InlineData(2)]
  public void ObeysLeftIdentityLaw(int value) {
    Some(value).SelectMany(SaveDiv1By)
      .Should().Be(SaveDiv1By(value));
  }

  [Theory]
  [InlineData("")]
  [InlineData("foo")]
  [InlineData("42")]
  [InlineData("1337")]
  public void ObeysRightIdentityLaw(string value) {
    Some(value).SelectMany(Some)
      .Should().Be(Some(value));
  }

  [Theory]
  [InlineData("bar")]
  [InlineData("-1")]
  [InlineData("0")]
  [InlineData("4")]
  public void ObeysAssociativityLaw(string value) {
    Some(value).SelectMany(TryParseInt).SelectMany(SaveDiv1By)
      .Should().Be(Some(value).SelectMany(x => TryParseInt(x).SelectMany(SaveDiv1By)));
  }

  static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
  static Maybe<T> None<T>() => Maybe<T>.None();
  static string ToString(int x) => x.ToString();
  static Maybe<string> ToMaybeString(int x) => Some(x.ToString());
  static string Id(string x) => x;
  static int GetLength(string x) => x.Length;
  static bool IsEven(int x) => x % 2 == 0;
  static Maybe<int> SaveDiv1By(int x) => x == 0 ? None<int>() : Some(1 / x);

  static Maybe<int> TryParseInt(string value) =>
    int.TryParse(s: value, result: out int result) ? Some(result) : None<int>();
}