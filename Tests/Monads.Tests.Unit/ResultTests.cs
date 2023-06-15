using FluentAssertions;
using Frognar.Monads;

namespace Monads.Tests.Unit;

public class ResultTests {
  [Fact]
  public void ReturnsSuccessWhenNoErrorIsProvided() {
    Result result = Result.Ok();

    result.IsSuccess.Should().BeTrue();
  }
}