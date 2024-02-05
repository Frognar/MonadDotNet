namespace Monads.Tests.Unit.MaybeTests;

public class MaybeTests {
  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
  public void FlattensNestedMaybe(int value) {
    Some(Some(value))
      .Flatten()
      .OrElse(-1)
      .Should().Be(value);
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.StringTestData))]
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
  [ClassData(typeof(Helpers.TestDataGenerators.StringTestData))]
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
  [ClassData(typeof(Helpers.TestDataGenerators.IntTestData))]
  public void ObeysLeftIdentityLaw(int value) {
    Some(value).SelectMany(SaveDiv1By)
      .Should().Be(SaveDiv1By(value));
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.StringTestData))]
  public void ObeysRightIdentityLaw(string value) {
    Some(value).SelectMany(Some)
      .Should().Be(Some(value));
  }

  [Theory]
  [ClassData(typeof(Helpers.TestDataGenerators.StringTestData))]
  public void ObeysAssociativityLaw(string value) {
    Some(value).SelectMany(TryParseInt).SelectMany(SaveDiv1By)
      .Should().Be(Some(value).SelectMany(x => TryParseInt(x).SelectMany(SaveDiv1By)));
  }
}