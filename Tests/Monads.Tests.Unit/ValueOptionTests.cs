using FluentAssertions;
using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class ValueOptionTests {
  [Fact]
  public void Some_ReturnsValueOptionWithGivenValue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    valueOption.OrElse(0).Should().Be(5);
  }

  [Fact]
  public void None_ReturnsValueOptionWithDefaultValue() {
    IOption<int> valueOption = ValueOption<int>.None();

    valueOption.OrElse(0).Should().Be(0);
  }

  [Fact]
  public void Map_TransformsValueInsideValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<string> mappedValueOption = valueOption.Map(i => i.ToString());

    mappedValueOption.OrElse("0").Should().Be("5");
  }

  [Fact]
  public void MapValue_TransformsValueInsideValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> mappedValueOption = valueOption.MapValue(i => i * 2);

    mappedValueOption.OrElse(0).Should().Be(10);
  }

  [Fact]
  public void FlatMap_TransformsValueInsideValueOptionToAnotherOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<string> flatMappedValueOption = valueOption.FlatMap(i => Option<string>.Some(i.ToString()));

    flatMappedValueOption.OrElse("").Should().Be("5");
  }

  [Fact]
  public void FlatMapValue_TransformsValueInsideValueOptionToAnotherValueOption() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> flatMappedValueOption = valueOption.FlatMapValue(i => ValueOption<int>.Some(i * 2));

    flatMappedValueOption.OrElse(0).Should().Be(10);
  }

  [Fact]
  public void Where_ReturnsValueOptionWithGivenValueWhenPredicateIsTrue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.Where(i => i > 0);

    filteredValueOption.OrElse(0).Should().Be(5);
  }

  [Fact]
  public void Where_ReturnsNoneWhenPredicateIsFalse() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.Where(i => i < 0);

    filteredValueOption.OrElse(0).Should().Be(0);
  }

  [Fact]
  public void WhereNot_ReturnsValueOptionWithGivenValueWhenPredicateIsFalse() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.WhereNot(i => i < 0);

    filteredValueOption.OrElse(0).Should().Be(5);
  }

  [Fact]
  public void WhereNot_ReturnsNoneWhenPredicateIsTrue() {
    IOption<int> valueOption = ValueOption<int>.Some(5);

    IOption<int> filteredValueOption = valueOption.WhereNot(i => i > 0);

    filteredValueOption.OrElse(0).Should().Be(0);
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothValueOptionsHaveSameValue() {
    IOption<int> valueOption1 = ValueOption<int>.Some(5);
    IOption<int> valueOption2 = ValueOption<int>.Some(5);

    valueOption1.Should().Be(valueOption2);
  }

  [Fact]
  public void Equals_ReturnsFalseWhenValueOptionsHaveDifferentValues() {
    IOption<int> valueOption1 = ValueOption<int>.Some(5);
    IOption<int> valueOption2 = ValueOption<int>.Some(10);

    valueOption1.Should().NotBe(valueOption2);
  }

  [Fact]
  public void Equals_ReturnsTrueWhenBothValueOptionsAreNone() {
    IOption<int> valueOption1 = ValueOption<int>.None();
    IOption<int> valueOption2 = ValueOption<int>.None();

    valueOption1.Should().Be(valueOption2);
  }

  [Fact]
  public void OnValue_WithValue_CallsAction() {
    bool wasCalled = false;
    void Action(int _) => wasCalled = true;
    IOption<int> option = ValueOption<int>.Some(1);

    option.IfPresent(Action);

    wasCalled.Should().BeTrue();
  }

  [Fact]
  public void OnValue_WithNull_DoesNotCallAction() {
    bool wasCalled = false;
    IOption<int> option = ValueOption<int>.None();

    option.IfPresent(Action);

    wasCalled.Should().BeFalse();
    void Action(int _) => wasCalled = true;
  }

  [Fact]
  public async Task OnValueAsync_WithValue_CallsAction() {
    bool wasCalled = false;

    Task Action(int _) {
      wasCalled = true;
      return Task.CompletedTask;
    }

    IOption<int> option = ValueOption<int>.Some(1);

    await option.IfPresentAsync(Action);

    wasCalled.Should().BeTrue();
  }

  [Fact]
  public async Task OnValueAsync_WithNull_DoesNotCallAction() {
    bool wasCalled = false;

    Task Action(int _) {
      wasCalled = true;
      return Task.CompletedTask;
    }

    IOption<int> option = ValueOption<int>.None();

    await option.IfPresentAsync(Action);

    wasCalled.Should().BeFalse();
  }
}