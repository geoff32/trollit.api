namespace Core.Exceptions;

public class ManagedException : Exception
{
    public ManagedException() { }
    public ManagedException(string message) : base(message) { }
    public ManagedException(string message, Exception inner) : base(message, inner) { }
}
