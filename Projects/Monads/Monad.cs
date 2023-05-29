using System;

namespace Frognar.Monads {
  public interface Monad<T> {
    Monad<U> Bind<U>(Func<T, Monad<U>> func);    
  }
}
