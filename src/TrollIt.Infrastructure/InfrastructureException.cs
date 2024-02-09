using Core.Exceptions;

namespace TrollIt.Infrastructure;

public class InfrastructureException<TErrorCategory>(TErrorCategory error) : ErrorCodeException<TErrorCategory>(error)
    where TErrorCategory : struct, Enum;