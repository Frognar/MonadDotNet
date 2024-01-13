namespace Frognar.Monads.Results;

public readonly struct Error {
  public readonly string Code;
  public readonly string Description;
  public readonly ErrorType Type;

  Error(string code, string description, ErrorType type) {
    Code = code;
    Description = description;
    Type = type;
  }

  public static Error Failure(string code = "Failure", string description = "A failure has occurred.") {
    return new Error(code, description, ErrorType.Failure);
  }

  public static Error Validation(string code = "Validation", string description = "A validation error has occurred.") {
    return new Error(code, description, ErrorType.Validation);
  }
}