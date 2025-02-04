using FluentAssertions.Execution;

namespace TrollIt.Api.Tests;

public static class HttpResponseMessageAssertionsExtensions
{
    public static HttpResponseMessageAssertions Should(this HttpResponseMessage subject) =>
        new(subject, AssertionChain.GetOrCreate());
}
