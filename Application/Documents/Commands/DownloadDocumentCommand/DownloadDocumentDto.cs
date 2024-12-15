namespace Application.Documents.Commands.DownloadDocumentCommand;

public record DownloadDocumentDto
{
    public required byte[] Content { get; init; }
    public required string Name { get; init; }
}