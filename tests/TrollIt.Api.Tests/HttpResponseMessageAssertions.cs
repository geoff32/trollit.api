using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace TrollIt.Api.Tests;

public class HttpResponseMessageAssertions(HttpResponseMessage subject, AssertionChain chain)
    : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>(subject, chain)
{
    protected override string Identifier => "HttpResponseMessage";
    
    public AndConstraint<HttpResponseMessageAssertions> HasJsonContent()
    {
        CurrentAssertionChain
            .ForCondition(Subject.Content.Headers.ContentType?.MediaType == "application/json")
            .FailWith("Expected {context:response} to have content type 'application/json'{reason}, but found '{0}'.",
                Subject.Content.Headers.ContentType?.MediaType);

        return new AndConstraint<HttpResponseMessageAssertions>(this);
    }
    
    public async Task VerifyContentAsync<T>(VerifySettings? verifySettings = null)
    {
        var result = await Subject.Content.ReadFromJsonAsync<T>();
        
        CurrentAssertionChain
            .ForCondition(result is not null)
            .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");
        
        await Verify(result, verifySettings ?? new VerifySettings());
    }
    
    public async Task VerifyContentAsync(VerifySettings? verifySettings = null)
    {
        var result = await Subject.Content.ReadAsStringAsync();
        
        await VerifyJson(result, verifySettings ?? new VerifySettings());
    }

    #region Status
    
    public AndConstraint<HttpResponseMessageAssertions> Be200Ok() => BeStatusCode(HttpStatusCode.OK);
    public AndConstraint<HttpResponseMessageAssertions> Be201Created() => BeStatusCode(HttpStatusCode.Created);
    public AndConstraint<HttpResponseMessageAssertions> Be204NoContent() => BeStatusCode(HttpStatusCode.NoContent);
    public AndConstraint<HttpResponseMessageAssertions> Be400BadRequest() => BeStatusCode(HttpStatusCode.BadRequest);
    public AndConstraint<HttpResponseMessageAssertions> Be401Unauthorized() => BeStatusCode(HttpStatusCode.Unauthorized);
    public AndConstraint<HttpResponseMessageAssertions> Be403Forbidden() => BeStatusCode(HttpStatusCode.Forbidden);
    public AndConstraint<HttpResponseMessageAssertions> Be404NotFound() => BeStatusCode(HttpStatusCode.NotFound);
    public AndConstraint<HttpResponseMessageAssertions> Be500InternalServerError() => BeStatusCode(HttpStatusCode.InternalServerError);
    

    private AndConstraint<HttpResponseMessageAssertions> BeStatusCode(HttpStatusCode statusCode)
    {
        CurrentAssertionChain
            .ForCondition(Subject!.StatusCode == statusCode)
            .FailWith("Expected {context:response} to have status code {0}{reason}, but found {1}.", statusCode,
                Subject.StatusCode);

        return new AndConstraint<HttpResponseMessageAssertions>(this);
    }

    #endregion
}