using Core.Exceptions;

namespace TrollIt.Domain;

public class DomainException<TErrorCategory>(TErrorCategory error) : ErrorCodeException<TErrorCategory>(error)
    where TErrorCategory : struct, Enum;
