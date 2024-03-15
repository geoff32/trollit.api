namespace Core.Exceptions;

public class ErrorCodeException(string context, string code) : ManagedException($"{code}")
{
    public string Context { get; } = context;
    public string Code { get; } = code;
}

public class ErrorCodeException<TErrorCategory>(TErrorCategory error) : ErrorCodeException(typeof(TErrorCategory).FullName!, Enum.GetName<TErrorCategory>(error) ?? error.ToString())
    where TErrorCategory : struct, Enum
{
    public override Type ExceptionType => typeof(TErrorCategory);
}
