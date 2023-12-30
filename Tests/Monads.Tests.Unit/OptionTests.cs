using FluentAssertions;
using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class OptionTests {

  [Fact]
  public void Some_ReturnsOptionWithGivenValue() {
    IOption<string> option = Option<string>.Some("test");

    option.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void None_ReturnsOptionWithDefaultValue() {
    IOption<string> option = Option<string>.None();

    option.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void Map_TransformsValueInsideOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> mappedOption = option.Map(s => s.ToUpper());

    mappedOption.OrElse("").Should().Be("TEST");
  }

  [Fact]
  public void MapValue_TransformsValueInsideOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<int> mappedOption = option.MapValue(s => s.Length);

    mappedOption.OrElse(0).Should().Be(4);
  }

  [Fact]
  public void FlatMap_TransformsValueInsideOptionToAnotherOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> flatMappedOption = option.FlatMap(s => Option<string>.Some(s.ToUpper()));

    Assert.Equal("TEST", flatMappedOption.OrElse(""));
  }

  [Fact]
  public void FlatMapValue_TransformsValueInsideOptionToAnotherValueOption() {
    IOption<string> option = Option<string>.Some("test");

    IOption<int> flatMappedOption = option.FlatMapValue(s => ValueOption<int>.Some(s.Length));

    flatMappedOption.OrElse(0).Should().Be(4);
  }

  [Fact]
  public void Where_ReturnsOptionWithGivenValueWhenPredicateIsTrue() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.Where(s => s.StartsWith("t"));

    filteredOption.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void Where_ReturnsNoneWhenPredicateIsFalse() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.Where(s => s.StartsWith("a"));

    filteredOption.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void WhereNot_ReturnsOptionWithGivenValueWhenPredicateIsFalse() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.WhereNot(s => s.StartsWith("a"));

    filteredOption.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void WhereNot_ReturnsNoneWhenPredicateIsTrue() {
    IOption<string> option = Option<string>.Some("test");

    IOption<string> filteredOption = option.WhereNot(s => s.StartsWith("t"));

    filteredOption.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsHaveSameValue() {
    IOption<string> option1 = Option<string>.Some("test");
    IOption<string> option2 = Option<string>.Some("test");

    option1.Should().Be(option2);
  }

  [Fact]
  public void Equals_ReturnsFalseWhenOptionsHaveDifferentValues() {
    IOption<string> option1 = Option<string>.Some("test");
    IOption<string> option2 = Option<string>.Some("other");

    option1.Should().NotBe(option2);
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsAreNone() {
    IOption<string> option1 = Option<string>.None();
    IOption<string> option2 = Option<string>.None();

    option1.Should().Be(option2);
  }

  [Fact]
  public void OnValue_WithValue_CallsAction() {
    bool wasCalled = false;
    void Action(string _) => wasCalled = true;
    IOption<string> option = Option<string>.Some("test");

    option.IfPresent(Action);

    wasCalled.Should().BeTrue();
  }

  [Fact]
  public void OnValue_WithNull_DoesNotCallAction() {
    bool wasCalled = false;
    IOption<string> option = Option<string>.None();

    option.IfPresent(Action);

    wasCalled.Should().BeFalse();
    void Action(string _) => wasCalled = true;
  }

  [Fact]
  public async Task OnValueAsync_WithValue_CallsAction() {
    bool wasCalled = false;

    Task Action(string _) {
      wasCalled = true;
      return Task.CompletedTask;
    }

    IOption<string> option = Option<string>.Some("test");

    await option.IfPresentAsync(Action);

    wasCalled.Should().BeTrue();
  }

  [Fact]
  public async Task OnValueAsync_WithNull_DoesNotCallAction() {
    bool wasCalled = false;

    Task Action(string _) {
      wasCalled = true;
      return Task.CompletedTask;
    }

    IOption<string> option = Option<string>.None();

    await option.IfPresentAsync(Action);

    wasCalled.Should().BeFalse();
  }
}