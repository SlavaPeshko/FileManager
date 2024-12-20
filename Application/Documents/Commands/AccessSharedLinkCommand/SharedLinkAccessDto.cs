namespace Application.Documents.Commands.AccessSharedLinkCommand;

public record SharedLinkAccessDto
{
    public required byte[] Content { get; init; }
    public required string Name { get; init; }
}