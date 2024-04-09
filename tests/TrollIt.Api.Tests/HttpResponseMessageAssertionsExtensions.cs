using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace TrollIt.Api.Tests;

public static class HttpResponseMessageAssertionsExtensions
{
    public static async Task<AndConstraint<ObjectAssertions>> BeStatusCode<T>(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
    {
        assertions.Subject.StatusCode.Should().Be(statusCode);
        var result = await assertions.Subject.Content.ReadFromJsonAsync<T>();

        return result.Should().NotBeNull().And.Subject.Should().BeOfType<T>().Which.Should().BeEquivalentTo(expected, config);
    }
    public static Task<AndConstraint<ObjectAssertions>> BeOk<T>(this HttpResponseMessageAssertions assertions, T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>>? config = null)
    {
        return assertions.BeStatusCode<T>(HttpStatusCode.OK, expected, config ?? (options => options));
    }

    public static Task<AndConstraint<ObjectAssertions>> BeProblemsDetailStatusCode(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, string expectedDetail, string expectedTitle = "Erreur")
    {
        return assertions.BeStatusCode(statusCode, new ProblemDetails
        {
            Title = expectedTitle,
            Detail = expectedDetail,
            Status = (int)statusCode
        }, options => options.Excluding(problem => problem.Extensions).Excluding(problem => problem.Type));
    }

    public static Task<AndConstraint<ObjectAssertions>> BeBadRequest(this HttpResponseMessageAssertions assertions, string expectedDetail, string expectedTitle = "Erreur")
    {
        return assertions.BeProblemsDetailStatusCode(HttpStatusCode.BadRequest, expectedDetail, expectedTitle);
    }

    public static Task<AndConstraint<ObjectAssertions>> BeUnauthorized(this HttpResponseMessageAssertions assertions, string expectedDetail, string expectedTitle = "Erreur")
    {
        return assertions.BeProblemsDetailStatusCode(HttpStatusCode.Unauthorized, expectedDetail, expectedTitle);
    }
}
