namespace TrollIt.Infrastructure.Mountyhall.Errors.Abstractions;

internal interface IPublicScriptErrorProvider
{
    void EnsureContent(string? content);
}