namespace Application.Documents.Commands.AccessSharedLinkCommand;

public class SharedLinkAccessDto
{
    public required byte[] Content { get; init; }
    public required string Name { get; init; }
}