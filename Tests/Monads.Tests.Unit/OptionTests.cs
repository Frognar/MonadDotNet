using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class OptionTests {

  [Fact]
  public void Some_ReturnsOptionWithGivenValue() {
    IOption<string> option = Option<string>.Some("test");

    Assert.Equal("test", option.Reduce("default"));
  }

  [Fact]
  public void None_ReturnsOptionWithDefaultValue() {
    IOption<string> option = Option<string>.None();

    Assert.Equal("default", option.Reduce("default"));
  }

  [Fact]
  public void Map_TransformsValueInsideOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> mappedOption = option.Map(s => s.ToUpper());

    Assert.Equal("TEST", mappedOption.Reduce(""));
  }

  [Fact]
  public void MapValue_TransformsValueInsideOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<int> mappedOption = option.MapValue(s => s.Length);

    Assert.Equal(4, mappedOption.Reduce(0));
  }

  [Fact]
  public void FlatMap_TransformsValueInsideOptionToAnotherOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> flatMappedOption = option.FlatMap(s => Option<string>.Some(s.ToUpper()));

    Assert.Equal("TEST", flatMappedOption.Reduce(""));
  }

  [Fact]
  public void FlatMapValue_TransformsValueInsideOptionToAnotherValueOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<int> flatMappedOption = option.FlatMapValue(s => ValueOption<int>.Some(s.Length));

    Assert.Equal(4, flatMappedOption.Reduce(0));
  }

  [Fact]
  public void Where_ReturnsOptionWithGivenValueWhenPredicateIsTrue() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.Where(s => s.StartsWith("t"));

    Assert.Equal("test", filteredOption.Reduce("default"));
  }

  [Fact]
  public void Where_ReturnsNoneWhenPredicateIsFalse() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.Where(s => s.StartsWith("a"));

    Assert.Equal("default", filteredOption.Reduce("default"));
  }

  [Fact]
  public void WhereNot_ReturnsOptionWithGivenValueWhenPredicateIsFalse() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.WhereNot(s => s.StartsWith("a"));

    Assert.Equal("test", filteredOption.Reduce("default"));
  }

  [Fact]
  public void WhereNot_ReturnsNoneWhenPredicateIsTrue() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.WhereNot(s => s.StartsWith("t"));

    Assert.Equal("default", filteredOption.Reduce("default"));
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsHaveSameValue() {
    IOption<string> option1 = Option<string>.Some("test");
    IOption<string> option2 = Option<string>.Some("test");

    Assert.True(option1.Equals(option2));
  }

  [Fact]
  public void Equals_ReturnsFalseWhenOptionsHaveDifferentValues() {
    IOption<string> option1 = Option<string>.Some("test");
    IOption<string> option2 = Option<string>.Some("other");

    Assert.False(option1.Equals(option2));
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsAreNone() {
    IOption<string> option1 = Option<string>.None();
    IOption<string> option2 = Option<string>.None();

    Assert.True(option1.Equals(option2));
  }
}