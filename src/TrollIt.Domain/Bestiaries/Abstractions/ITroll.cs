namespace TrollIt.Domain.Bestiaries.Abstractions;

public interface ITroll
{
    int Id { get; }
    string Name { get; }
    IBreed Breed { get; }
    IGuild? Guild { get; }
}
