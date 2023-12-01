namespace Frognar.Monads.Optional;

public readonly struct ValueOption<T> where T : struct
{
    private readonly T? value;

    ValueOption(T? value)
    {
        this.value = value;
    }

    public static ValueOption<T> Some(T obj) => new(obj);
    public static ValueOption<T> None => new(null);
}