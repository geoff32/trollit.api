namespace TrollIt.Infrastructure.Ftp.Errors;

internal enum PublicScriptErrorCodes
{
    Unknown = 0,
    IncorrectParameter = 1,
    UnknownTroll = 2,
    WrongPassword = 3,
    Maintenance = 4,
    ScriptTemporarilyDisabled = 5,
    PnjOrTrollDisabled = 6
}