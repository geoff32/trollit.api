using TrollIt.Infrastructure.Mountyhall.Errors.Abstractions;

namespace TrollIt.Infrastructure.Mountyhall.Readers.Errors;

internal class PublicScriptErrorProvider : IPublicScriptErrorProvider
{
    public void EnsureContent(string? content)
    {
        if (content != null && HasError(content))
        {
            throw new InfrastructureException<PublicScriptErrorCodes>(GetErrorCodes(content));
        }
    }

    private static bool HasError(string content) => content.StartsWith("Erreur");

    private static PublicScriptErrorCodes GetErrorCodes(string content)
    {
        if (Enum.TryParse<PublicScriptErrorCodes>(content
                .Split(":", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                ?.Split(" ", StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), out var errorCode))
        {
            return errorCode;
        }

        return PublicScriptErrorCodes.Unknown;
    }
}