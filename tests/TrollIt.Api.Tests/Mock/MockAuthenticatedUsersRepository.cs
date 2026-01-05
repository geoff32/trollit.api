using System.Diagnostics.CodeAnalysis;
using TrollIt.Application;

namespace TrollIt.Api.Tests;

public class MockAuthenticatedUsersRepository
{
    private readonly Dictionary<Guid, AppUser> _users = [];

    public void AddUser(AppUser user) => _users.Add(user.AccountId, user);
    public void RemoveUser(Guid userId) => _users.Remove(userId);
    public bool IsUserAuthenticated(Guid userId, [NotNullWhen(true)] out AppUser? user) => _users.TryGetValue(userId, out user);

    public void Clear() => _users.Clear();
}
