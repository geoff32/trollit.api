namespace TrollIt.Infrastructure.Ftp.Errors.Abstractions;

internal interface IPublicScriptErrorProvider
{
    void EnsureContent(string? content);
}