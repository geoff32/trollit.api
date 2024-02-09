using Core.Exceptions;

namespace TrollIt.Application;

public class AppException<TErrorCategory>(TErrorCategory error) : ErrorCodeException<TErrorCategory>(error)
    where TErrorCategory : struct, Enum;