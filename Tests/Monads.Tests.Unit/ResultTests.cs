using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class ResultTests {
  [Fact]
  public void ReturnsSuccessWhenValueIsProvided() {
    const string value = "test value";
    Result<string> result = Result<string>.Ok(value);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void ReturnsFailureWhenErrorIsProvided() {
    Exception error = new("test error");
    Result<string> result = Result<string>.Fail(error);

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(error);
  }
}