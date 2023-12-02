using Frognar.Monads.Optional;

namespace Monads.Tests.Unit;

public class OptionTests
{
    
    [Fact]
    public void Some_ReturnsOptionWithGivenValue()
    {
        var option = Option<string>.Some("test");

        Assert.Equal("test", option.Reduce("default"));
    }

    [Fact]
    public void None_ReturnsOptionWithDefaultValue()
    {
        var option = Option<string>.None();

        Assert.Equal("default", option.Reduce("default"));
    }

    [Fact]
    public void Map_TransformsValueInsideOption()
    {
        var option = Option<string>.Some("test");

        var mappedOption = option.Map(s => s.ToUpper());

        Assert.Equal("TEST", mappedOption.Reduce(""));
    }

    [Fact]
    public void MapValue_TransformsValueInsideOption()
    {
        var option = Option<string>.Some("test");

        var mappedOption = option.MapValue(s => s.Length);

        Assert.Equal(4, mappedOption.Reduce(0));
    }

    [Fact]
    public void FlatMap_TransformsValueInsideOptionToAnotherOption()
    {
        var option = Option<string>.Some("test");

        var flatMappedOption = option.FlatMap(s => Option<string>.Some(s.ToUpper()));

        Assert.Equal("TEST", flatMappedOption.Reduce(""));
    }

    [Fact]
    public void FlatMapValue_TransformsValueInsideOptionToAnotherValueOption()
    {
        var option = Option<string>.Some("test");

        var flatMappedOption = option.FlatMapValue(s => ValueOption<int>.Some(s.Length));

        Assert.Equal(4, flatMappedOption.Reduce(0));
    }

    [Fact]
    public void Where_ReturnsOptionWithGivenValueWhenPredicateIsTrue()
    {
        var option = Option<string>.Some("test");

        var filteredOption = option.Where(s => s.StartsWith("t"));

        Assert.Equal("test", filteredOption.Reduce("default"));
    }

    [Fact]
    public void Where_ReturnsNoneWhenPredicateIsFalse()
    {
        var option = Option<string>.Some("test");

        var filteredOption = option.Where(s => s.StartsWith("a"));

        Assert.Equal("default", filteredOption.Reduce("default"));
    }

    [Fact]
    public void WhereNot_ReturnsOptionWithGivenValueWhenPredicateIsFalse()
    {
        var option = Option<string>.Some("test");

        var filteredOption = option.WhereNot(s => s.StartsWith("a"));

        Assert.Equal("test", filteredOption.Reduce("default"));
    }

    [Fact]
    public void WhereNot_ReturnsNoneWhenPredicateIsTrue()
    {
        var option = Option<string>.Some("test");

        var filteredOption = option.WhereNot(s => s.StartsWith("t"));

        Assert.Equal("default", filteredOption.Reduce("default"));
    }

    [Fact]
    public void Equals_ReturnsTrueWhenBothOptionsHaveSameValue()
    {
        var option1 = Option<string>.Some("test");
        var option2 = Option<string>.Some("test");

        Assert.True(option1.Equals(option2));
    }

    [Fact]
    public void Equals_ReturnsFalseWhenOptionsHaveDifferentValues()
    {
        var option1 = Option<string>.Some("test");
        var option2 = Option<string>.Some("other");

        Assert.False(option1.Equals(option2));
    }

    [Fact]
    public void Equals_ReturnsTrueWhenBothOptionsAreNone()
    {
        var option1 = Option<string>.None();
        var option2 = Option<string>.None();

        Assert.True(option1.Equals(option2));
    }
}