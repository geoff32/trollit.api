using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace TrollIt.Api.Tests;

public static class HttpResponseMessageAssertionsExtensions
{
    public static async Task<AndConstraint<ObjectAssertions>> BeBadRequest<T>(this HttpResponseMessageAssertions assertions, T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
    {
        assertions.Subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await assertions.Subject.Content.ReadFromJsonAsync<T>();

        return result.Should().NotBeNull().And.Subject.Should().BeOfType<T>().Which.Should().BeEquivalentTo(expected, config);
    }

    public static Task<AndConstraint<ObjectAssertions>> BeBadRequest(this HttpResponseMessageAssertions assertions, string expectedDetail, string expectedTitle = "Erreur")
    {
        return assertions.BeBadRequest(new ProblemDetails
        {
            Title = expectedTitle,
            Detail = expectedDetail,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Status = 400
        }, options => options.Excluding(problem => problem.Extensions));
    }
}
