using Frognar.Monads;

int result = 2
  .Run(AddOne)
  .Run(Square)
  .Value;

Maybe<int> AddOne(int x) => x + 1;
Maybe<int> Square(int x) => x * x;

Console.WriteLine(result);