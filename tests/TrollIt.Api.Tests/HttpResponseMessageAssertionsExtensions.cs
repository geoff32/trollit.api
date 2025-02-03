using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace TrollIt.Api.Tests;

public static class HttpResponseMessageAssertionsExtensions
{
    public static async Task BeStatusCode(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, VerifySettings? settings = null)
    {
        assertions.Subject.StatusCode.Should().Be(statusCode);
        var result = await assertions.Subject.Content.ReadAsStringAsync();

        result.Should().NotBeNull();
        await Verify(result, settings);
    }
    public static async Task BeStatusCode<T>(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, VerifySettings? settings = null)
    {
        assertions.Subject.StatusCode.Should().Be(statusCode);
        var result = await assertions.Subject.Content.ReadFromJsonAsync<T>();

        result.Should().NotBeNull().And.Subject.Should().BeOfType<T>();
        await Verify(result, settings);
    }
    
    public static async Task<AndConstraint<ObjectAssertions>> BeStatusCode<T>(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
    {
        assertions.Subject.StatusCode.Should().Be(statusCode);
        var result = await assertions.Subject.Content.ReadFromJsonAsync<T>();

        return result.Should().NotBeNull().And.Subject.Should().BeOfType<T>().Which.Should().BeEquivalentTo(expected, config);
    }

    public static Task BeOk(this HttpResponseMessageAssertions assertions,
        VerifySettings? settings = null) =>
        assertions.BeStatusCode(HttpStatusCode.OK, settings);
    
    public static Task BeOk<T>(this HttpResponseMessageAssertions assertions,
        VerifySettings? settings = null) =>
        assertions.BeStatusCode<T>(HttpStatusCode.OK, settings);

    public static Task<AndConstraint<ObjectAssertions>> BeProblemsDetailStatusCode(this HttpResponseMessageAssertions assertions, HttpStatusCode statusCode, string expectedDetail, string expectedTitle = "Erreur")
    {
        return assertions.BeStatusCode(statusCode, new ProblemDetails
        {
            Title = expectedTitle,
            Detail = expectedDetail,
            Status = (int)statusCode
        }, options => options.Excluding(problem => problem.Extensions).Excluding(problem => problem.Type));
    }

    public static Task BeBadRequest(this HttpResponseMessageAssertions assertions, VerifySettings? settings = null) =>
        assertions.BeStatusCode<ProblemDetails>(HttpStatusCode.BadRequest, settings);

    public static Task BeUnauthorized(this HttpResponseMessageAssertions assertions, VerifySettings? settings = null) =>
        assertions.BeStatusCode<ProblemDetails>(HttpStatusCode.Unauthorized, settings);
    
    public static Task BeForbidden(this HttpResponseMessageAssertions assertions, VerifySettings? settings = null) =>
        assertions.BeStatusCode<ProblemDetails>(HttpStatusCode.Forbidden, settings);
    
    public static Task BeNotFound(this HttpResponseMessageAssertions assertions, VerifySettings? settings = null) =>
        assertions.BeStatusCode<ProblemDetails>(HttpStatusCode.NotFound, settings);
}
