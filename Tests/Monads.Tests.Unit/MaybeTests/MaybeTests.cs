namespace Monads.Tests.Unit.MaybeTests;

public class MaybeTests {
  [Fact]
  public void FlattensNestedMaybe() {
    Some(Some(10))
      .Flatten()
      .OrElse(-1)
      .Should().Be(10);
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

  static string Id(string x) => x;
  static int GetLength(string x) => x.Length;
  static bool IsEven(int x) => x % 2 == 0;
  static Maybe<int> SaveDiv1By(int x) => x == 0 ? None<int>() : Some(1 / x);

  static Maybe<int> TryParseInt(string value) =>
    int.TryParse(s: value, result: out int result) ? Some(result) : None<int>();
}