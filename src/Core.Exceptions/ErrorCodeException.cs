namespace Core.Exceptions;

public class ErrorCodeException(string context, string code) : ManagedException($"{code}")
{
    public string Context { get; } = context;
    public string Code { get; } = code;
}

public class ErrorCodeException<TErrorCategory>(TErrorCategory errorCode) : ErrorCodeException(typeof(TErrorCategory).FullName!, Enum.GetName<TErrorCategory>(errorCode) ?? errorCode.ToString())
    where TErrorCategory : struct, Enum
{
    public override Type ExceptionType => typeof(TErrorCategory);
    public TErrorCategory ErrorCode => errorCode;
}
