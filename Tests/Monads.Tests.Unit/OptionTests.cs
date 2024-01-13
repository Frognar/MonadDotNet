using FluentAssertions;
using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class OptionTests {

  [Fact]
  public void Some_ReturnsOptionWithGivenValue() {
    Option<string> option = Option<string>.Some("test");

    option.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void None_ReturnsOptionWithDefaultValue() {
    Option<string> option = Option<string>.None();

    option.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void Map_TransformsValueInsideOption() {
    Option<string> option = Option<string>.Some("test");

    Option<string> mappedOption = option.Map(s => s.ToUpper());

    mappedOption.OrElse("").Should().Be("TEST");
  }

  [Fact]
  public void FlatMap_TransformsValueInsideOptionToAnotherOption() {
    Option<string> option = Option<string>.Some("test");

    Option<string> flatMappedOption = option.FlatMap(s => Option<string>.Some(s.ToUpper()));

    Assert.Equal("TEST", flatMappedOption.OrElse(""));
  }

  [Fact]
  public void Where_ReturnsOptionWithGivenValueWhenPredicateIsTrue() {
    Option<string> option = Option<string>.Some("test");

    Option<string> filteredOption = option.Where(s => s.StartsWith("t"));

    filteredOption.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void Where_ReturnsNoneWhenPredicateIsFalse() {
    Option<string> option = Option<string>.Some("test");

    Option<string> filteredOption = option.Where(s => s.StartsWith("a"));

    filteredOption.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void WhereNot_ReturnsOptionWithGivenValueWhenPredicateIsFalse() {
    Option<string> option = Option<string>.Some("test");

    Option<string> filteredOption = option.WhereNot(s => s.StartsWith("a"));

    filteredOption.OrElse("default").Should().Be("test");
  }

  [Fact]
  public void WhereNot_ReturnsNoneWhenPredicateIsTrue() {
    Option<string> option = Option<string>.Some("test");

    Option<string> filteredOption = option.WhereNot(s => s.StartsWith("t"));

    filteredOption.OrElse("default").Should().Be("default");
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsHaveSameValue() {
    Option<string> option1 = Option<string>.Some("test");
    Option<string> option2 = Option<string>.Some("test");

    option1.Should().Be(option2);
  }

  [Fact]
  public void Equals_ReturnsFalseWhenOptionsHaveDifferentValues() {
    Option<string> option1 = Option<string>.Some("test");
    Option<string> option2 = Option<string>.Some("other");

    option1.Should().NotBe(option2);
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothOptionsAreNone() {
    Option<string> option1 = Option<string>.None();
    Option<string> option2 = Option<string>.None();

    option1.Should().Be(option2);
  }

  [Fact]
  public void OnValue_WithValue_CallsAction() {
    bool wasCalled = false;
    void Action(string _) => wasCalled = true;
    Option<string> option = Option<string>.Some("test");

    option.IfPresent(Action);

    wasCalled.Should().BeTrue();
  }

  [Fact]
  public void OnValue_WithNull_DoesNotCallAction() {
    bool wasCalled = false;
    Option<string> option = Option<string>.None();

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

    Option<string> option = Option<string>.Some("test");

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

    Option<string> option = Option<string>.None();

    await option.IfPresentAsync(Action);

    wasCalled.Should().BeFalse();
  }
}