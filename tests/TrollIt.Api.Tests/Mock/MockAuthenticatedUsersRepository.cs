namespace TrollIt.Api.Tests;

public class MockAuthenticatedUsersRepository
{
    private readonly List<Guid> _users = [];

    public void AddUser(Guid userId) => _users.Add(userId);
    public void RemoveUser(Guid userId) => _users.Remove(userId);
    public bool IsUserAuthenticated(Guid userId) => _users.Contains(userId);
    public void Clear() => _users.Clear();
}
