namespace Core.Exceptions;

public class ErrorCodeException(string? category, string code) : ManagedException($"{category} : {code}")
{
    public string Category { get; } = category ?? "Unknown";
    public string Code { get; } = code;
}

public class ErrorCodeException<TErrorCategory>(TErrorCategory error) : ErrorCodeException(typeof(TErrorCategory).AssemblyQualifiedName, Enum.GetName<TErrorCategory>(error) ?? error.ToString())
    where TErrorCategory : struct, Enum
{
}
