using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class ValueOptionTests
{
    [Fact]
    public void Some_ReturnsValueOptionWithGivenValue()
    {
        var valueOption = ValueOption<int>.Some(5);

        Assert.Equal(5, valueOption.Reduce(0));
    }

    [Fact]
    public void None_ReturnsValueOptionWithDefaultValue()
    {
        var valueOption = ValueOption<int>.None();

        Assert.Equal(0, valueOption.Reduce(0));
    }

    [Fact]
    public void Map_TransformsValueInsideValueOption()
    {
        var valueOption = ValueOption<int>.Some(5);

        var mappedValueOption = valueOption.Map(i => i.ToString());

        Assert.Equal("5", mappedValueOption.Reduce("0"));
    }

    [Fact]
    public void MapValue_TransformsValueInsideValueOption()
    {
        var valueOption = ValueOption<int>.Some(5);

        var mappedValueOption = valueOption.MapValue(i => i * 2);

        Assert.Equal(10, mappedValueOption.Reduce(0));
    }

    [Fact]
    public void FlatMap_TransformsValueInsideValueOptionToAnotherOption()
    {
        var valueOption = ValueOption<int>.Some(5);

        var flatMappedValueOption = valueOption.FlatMap(i => Option<string>.Some(i.ToString()));

        Assert.Equal("5", flatMappedValueOption.Reduce(""));
    }

    [Fact]
    public void FlatMapValue_TransformsValueInsideValueOptionToAnotherValueOption()
    {
        var valueOption = ValueOption<int>.Some(5);

        var flatMappedValueOption = valueOption.FlatMapValue(i => ValueOption<int>.Some(i * 2));

        Assert.Equal(10, flatMappedValueOption.Reduce(0));
    }

    [Fact]
    public void Where_ReturnsValueOptionWithGivenValueWhenPredicateIsTrue()
    {
        var valueOption = ValueOption<int>.Some(5);

        var filteredValueOption = valueOption.Where(i => i > 0);

        Assert.Equal(5, filteredValueOption.Reduce(0));
    }

    [Fact]
    public void Where_ReturnsNoneWhenPredicateIsFalse()
    {
        var valueOption = ValueOption<int>.Some(5);

        var filteredValueOption = valueOption.Where(i => i < 0);

        Assert.Equal(0, filteredValueOption.Reduce(0));
    }

    [Fact]
    public void WhereNot_ReturnsValueOptionWithGivenValueWhenPredicateIsFalse()
    {
        var valueOption = ValueOption<int>.Some(5);

        var filteredValueOption = valueOption.WhereNot(i => i < 0);

        Assert.Equal(5, filteredValueOption.Reduce(0));
    }

    [Fact]
    public void WhereNot_ReturnsNoneWhenPredicateIsTrue()
    {
        var valueOption = ValueOption<int>.Some(5);

        var filteredValueOption = valueOption.WhereNot(i => i > 0);

        Assert.Equal(0, filteredValueOption.Reduce(0));
    }

    [Fact]
    public void Equals_ReturnsTrueWhenBothValueOptionsHaveSameValue()
    {
        var valueOption1 = ValueOption<int>.Some(5);
        var valueOption2 = ValueOption<int>.Some(5);

        Assert.True(valueOption1.Equals(valueOption2));
    }

    [Fact]
    public void Equals_ReturnsFalseWhenValueOptionsHaveDifferentValues()
    {
        var valueOption1 = ValueOption<int>.Some(5);
        var valueOption2 = ValueOption<int>.Some(10);

        Assert.False(valueOption1.Equals(valueOption2));
    }

    [Fact]
    public void Equals_ReturnsTrueWhenBothValueOptionsAreNone()
    {
        var valueOption1 = ValueOption<int>.None();
        var valueOption2 = ValueOption<int>.None();

        Assert.True(valueOption1.Equals(valueOption2));
    }
}
