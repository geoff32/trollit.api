namespace TrollIt.Application.Accounts.Models;

public record AuthenticateRequest(string UserName, string Password);