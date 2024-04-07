using System.Diagnostics.CodeAnalysis;
using System.Net;
using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace TrollIt.Api.Exceptions;

public class ManagedExceptionErrorHandler(ProblemDetailsFactory problemDetailsFactory, IProblemDetailsService problemDetailsService, IStringLocalizerFactory stringLocalizerFactory) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (TryGetManagedException(exception, out var managedException))
        {
            return await TryWriteAsync(httpContext, exception, managedException);
        }

        return false;
    }

    private static bool TryGetManagedException(Exception? exception, [NotNullWhen(true)] out ManagedException? managedException)
    {
        if (exception == null) {
            managedException = null;
            return false;
        }

        if (exception is ManagedException managedEx)
        {
            managedException = managedEx;
            return true;
        }

        return TryGetManagedException(exception.InnerException, out managedException);
    }

    private async ValueTask<bool> TryWriteAsync(HttpContext httpContext, Exception exception, ManagedException managedException)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = GetProblemDetails(httpContext, managedException),
            Exception = exception
        });
    }

    private ProblemDetails GetProblemDetails(HttpContext httpContext, ManagedException managedException)
    {
        var stringLocalizer = stringLocalizerFactory.Create(managedException.ExceptionType);
        var detail = stringLocalizer[$"{managedException.Message}"];
        return problemDetailsFactory.CreateProblemDetails
        (
            httpContext,
            statusCode: StatusCodes.Status400BadRequest,
            title: stringLocalizer["Title error"],
            detail: detail.ResourceNotFound ? stringLocalizer["Unknown error"] : detail
        );
    }
}
