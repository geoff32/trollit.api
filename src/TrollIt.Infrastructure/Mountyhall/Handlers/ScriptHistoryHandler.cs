using Microsoft.AspNetCore.WebUtilities;
using TrollIt.Application.Scripts.Abstractions;
using TrollIt.Infrastructure.Mountyhall.Exceptions;

namespace TrollIt.Infrastructure.Mountyhall.Handlers;

public class ScriptHistoryHandler(IScriptsService scriptsService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var queryParams = QueryHelpers.ParseQuery(request.RequestUri?.Query);
        if (queryParams.TryGetValue("Numero", out var paramTrollId)
            && int.TryParse(paramTrollId, out var trollId))
        {
            var scriptPath = request.RequestUri?.LocalPath;
            if (scriptPath is not null)
            {
                await scriptsService.TraceAsync(trollId, scriptPath, cancellationToken);

                return await base.SendAsync(request, cancellationToken);
            }
        }

        throw new InfrastructureException<MountyhallExceptions>(MountyhallExceptions.InvalidScript);
    }
}
