using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class ValueOptionTests {
  [Fact]
  public void Some_ReturnsValueOptionWithGivenValue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    Assert.Equal(5, valueOption.Reduce(0));
  }

  [Fact]
  public void None_ReturnsValueOptionWithDefaultValue() {
    IOption<int> valueOption = ValueOption<int>.None();

    Assert.Equal(0, valueOption.Reduce(0));
  }

  [Fact]
  public void Map_TransformsValueInsideValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<string> mappedValueOption = valueOption.Map(i => i.ToString());

    Assert.Equal("5", mappedValueOption.Reduce("0"));
  }

  [Fact]
  public void MapValue_TransformsValueInsideValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> mappedValueOption = valueOption.MapValue(i => i * 2);

    Assert.Equal(10, mappedValueOption.Reduce(0));
  }

  [Fact]
  public void FlatMap_TransformsValueInsideValueOptionToAnotherOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<string> flatMappedValueOption = valueOption.FlatMap(i => Option<string>.Some(i.ToString()));

    Assert.Equal("5", flatMappedValueOption.Reduce(""));
  }

  [Fact]
  public void FlatMapValue_TransformsValueInsideValueOptionToAnotherValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> flatMappedValueOption = valueOption.FlatMapValue(i => ValueOption<int>.Some(i * 2));

    Assert.Equal(10, flatMappedValueOption.Reduce(0));
  }

  [Fact]
  public void Where_ReturnsValueOptionWithGivenValueWhenPredicateIsTrue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.Where(i => i > 0);

    Assert.Equal(5, filteredValueOption.Reduce(0));
  }

  [Fact]
  public void Where_ReturnsNoneWhenPredicateIsFalse() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.Where(i => i < 0);

    Assert.Equal(0, filteredValueOption.Reduce(0));
  }

  [Fact]
  public void WhereNot_ReturnsValueOptionWithGivenValueWhenPredicateIsFalse() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.WhereNot(i => i < 0);

    Assert.Equal(5, filteredValueOption.Reduce(0));
  }

  [Fact]
  public void WhereNot_ReturnsNoneWhenPredicateIsTrue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.WhereNot(i => i > 0);

    Assert.Equal(0, filteredValueOption.Reduce(0));
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothValueOptionsHaveSameValue() {
    IOption<int> valueOption1 = ValueOption<int>.Some(5);
    IOption<int> valueOption2 = ValueOption<int>.Some(5);

    Assert.True(valueOption1.Equals(valueOption2));
  }

  [Fact]
  public void Equals_ReturnsFalseWhenValueOptionsHaveDifferentValues() {
    IOption<int> valueOption1 = ValueOption<int>.Some(5);
    IOption<int> valueOption2 = ValueOption<int>.Some(10);

    Assert.False(valueOption1.Equals(valueOption2));
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothValueOptionsAreNone() {
    IOption<int> valueOption1 = ValueOption<int>.None();
    IOption<int> valueOption2 = ValueOption<int>.None();

    Assert.True(valueOption1.Equals(valueOption2));
  }
}