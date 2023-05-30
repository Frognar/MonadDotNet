using Frognar.Monads;

int result = 2
  .Run(AddOne)
  .Run(Square)
  .Value;

Console.WriteLine(result);

Maybe<int> AddOne(int x) => x + 1;
Maybe<int> Square(int x) => x * x;

Logged<int> loggedResult = 2
  .RunWithLogs(AddOneWithLog)
  .RunWithLogs(SquareWithLog);

Console.WriteLine(loggedResult.Value);
Console.WriteLine(string.Join(Environment.NewLine, loggedResult.Logs));

Logged<int> AddOneWithLog(int x) => Logged<int>.Of(x + 1, $"Added 1 to {x} to get {x + 1}");
Logged<int> SquareWithLog(int x) => Logged<int>.Of(x * x, $"Squared {x} to get {x * x}");

Result<int> r = 2
  .Map(TryAddOne)
  .Map(TrySquare)
  .Map(TryError)
  .Map(TryMultiplyBy8);

r.Resolve(Console.WriteLine, ex => Console.WriteLine(ex.ToString()));

Result<int> TryAddOne(int x) => x + 1;
Result<int> TrySquare(int x) => x * x;
Result<int> TryError(int x) => new Exception();
Result<int> TryMultiplyBy8(int x) => x * 8;
