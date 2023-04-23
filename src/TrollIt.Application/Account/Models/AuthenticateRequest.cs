namespace TrollIt.Application.Account.Models;

public record AuthenticateRequest(string UserName, string Password);