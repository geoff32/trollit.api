using TrollIt.Domain;

namespace FluentAssertions.Specialized;

internal static class ErrorCodeExceptionExtensions
{
    public static void ThrowDomainException<TErrorCategory>(this ActionAssertions actionAssertions, TErrorCategory error)
        where TErrorCategory : struct, Enum
    {
        actionAssertions.Throw<DomainException<TErrorCategory>>().And.ErrorCode.Should().Be(error);
    }
}
