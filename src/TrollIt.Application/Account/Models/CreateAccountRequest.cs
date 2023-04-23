namespace TrollIt.Application.Account.Models;

public record CreateAccountRequest(string UserName, string Password, int TrollId, string Token);