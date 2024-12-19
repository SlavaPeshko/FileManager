namespace Application.Documents.Commands.CreateSharedLinkCommand;

public record CreateSharedLinkDto
{
    public required string UniqueKey { get; init; }
    public DateTimeOffset ExpirationDate { get; init; }
}