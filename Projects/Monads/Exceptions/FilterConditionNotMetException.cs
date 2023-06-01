using System;

namespace Frognar.Monads.Exceptions;

public class FilterConditionNotMetException : Exception {
  public FilterConditionNotMetException() {
  }

  public FilterConditionNotMetException(string message)
    : base(message) {
  }

  public FilterConditionNotMetException(string message, Exception innerException)
    : base(message, innerException) {
  }
}