using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class ResultTests {
  [Fact]
  public void ReturnsSuccessWhenNoErrorIsProvided() {
    Result result = Result.Ok();

    result.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public void ReturnsFailureWhenErrorIsProvided() {
    Exception error = new("test error");
    Result result = Result.Fail(error);

    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be(error);
  }
}