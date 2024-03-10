namespace TrollIt.Application.Accounts.Models;

public record CreateAccountRequest(string UserName, string Password, int TrollId, string Token);