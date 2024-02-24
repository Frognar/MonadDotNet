namespace Monads.Tests.Unit.EitherTests;

public class EitherTests {
  public static IEnumerable<object[]> EitherData
  {
    get
    {
      yield return [Either.Left<string, int>("foo")];
      yield return [Either.Left<string, int>("bar")];
      yield return [Either.Left<string, int>("baz")];
      yield return [Either.Right<string, int>(42)];
      yield return [Either.Right<string, int>(1337)];
      yield return [Either.Right<string, int>(0)];
    }
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysFirstFunctorLaw(Either<string, int> either) {
    either.LMap(l => l).Should().Be(either);
    either.RMap(r => r).Should().Be(either);
    either.BMap(lMap: l => l, rMap: r => r).Should().Be(either);
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysSecondFunctorLaw(Either<string, int> either) {
    either.BMap(lMap: IsNullOrWhiteSpace, rMap: ToDateTime)
      .Should().Be(either.RMap(ToDateTime).LMap(IsNullOrWhiteSpace));

    either.LMap(IsNullOrWhiteSpace).RMap(ToDateTime)
      .Should().Be(either.RMap(ToDateTime).LMap(IsNullOrWhiteSpace));
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysLeftIdentityLawWithSelectLeft(Either<string, int> either) {
    either.LMap(x => IsEven(GetLength(x)))
      .Should().Be(either.LMap(GetLength).LMap(IsEven));
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysLeftIdentityLawWithSelectRight(Either<string, int> either) {
    either.RMap(x => ToChar(IsEven(x)))
      .Should().Be(either.RMap(IsEven).RMap(ToChar));
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysLeftIdentityLawWithSelectBoth(Either<string, int> either) {
    either.BMap(lMap: x => IsEven(GetLength(x)), rMap: y => ToChar(IsEven(y)))
      .Should().Be(either.BMap(lMap: GetLength, rMap: IsEven)
        .BMap(lMap: IsEven, rMap: ToChar));
  }

  [Fact]
  public void ObeysRightIdentityLaw() {
    Either<int, string> right1 = Either.Left<int, string>(42);
    right1.RFlatMap(Either.Right<int, string>).Should().Be(right1);
    Either<int, string> right2 = Either.Right<int, string>("42");
    right2.RFlatMap(Either.Right<int, string>).Should().Be(right2);
    Either<int, string> left1 = Either.Left<int, string>(42);
    left1.LFlatMap(Either.Left<int, string>).Should().Be(left1);
    Either<int, string> left2 = Either.Right<int, string>("42");
    left2.LFlatMap(Either.Left<int, string>).Should().Be(left2);
    Either<int, string> either = Either.Left<int, string>(42);
    either.BFlatMap(
      lMap: Either.Left<int, string>,
      rMap: Either.Right<int, string>
    ).Should().Be(either);
  }

  [Theory]
  [MemberData(nameof(EitherData))]
  public void ObeysAssociativityLaw(Either<string, int> either) {
    either.RFlatMap(TryDiv1By).RFlatMap(Nat)
      .Should().Be(either.RFlatMap(x => TryDiv1By(x).RFlatMap(Nat)));
  }

  static bool IsEven(int i) => i % 2 == 0;
  static bool IsNullOrWhiteSpace(string s) => string.IsNullOrWhiteSpace(s);
  static DateTime ToDateTime(int i) => new(i);
  static char ToChar(bool b) => b ? 'T' : 'F';
  static int GetLength(string s) => s.Length;

  static Either<string, double> TryDiv1By(int value)
    => value == 0
      ? Either.Left<string, double>("Cannot divide by 0")
      : Either.Right<string, double>(1d / value);

  static Either<string, int> Nat(double d)
    => d % 1 != 0
      ? Either.Left<string, int>($"Non-integers not allowed: {d}.")
      : d < 1
        ? Either.Left<string, int>($"Non-positive numbers not allowed: {d}.")
        : Either.Right<string, int>((int)d);
}