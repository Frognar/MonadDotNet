﻿namespace Monads.Tests.Unit.EitherTests;

public class EitherSelectTests {
  [Fact]
  public void LeftIdentityLaw() {
    Either<int, string> either = Either<int, string>.Left(42);
    either.SelectLeft(x => x).Should().Be(either);
  }

  [Fact]
  public void MapLeftWhenLeftCreated() {
    Either<int, string>.Left(42)
      .SelectLeft(x => x.ToString())
      .Match(left => left, _ => "")
      .Should().Be("42");
  }
}