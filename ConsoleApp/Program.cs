using Frognar.Monads;

int result = 2
  .Run(AddOne)
  .Run(Square)
  .Value;

Maybe<int> AddOne(int x) => x + 1;
Maybe<int> Square(int x) => x * x;
Console.WriteLine(result);

Logged<int> loggedResult = 2
  .RunWithLogs(AddOneWithLog)
  .RunWithLogs(SquareWithLog);

Logged<int> AddOneWithLog(int x) => Logged<int>.Of(x + 1, $"Added 1 to {x} to get {x + 1}");
Logged<int> SquareWithLog(int x) => Logged<int>.Of(x * x, $"Squared {x} to get {x * x}");
Console.WriteLine(loggedResult.Value);
Console.WriteLine(string.Join(Environment.NewLine, loggedResult.Logs));
