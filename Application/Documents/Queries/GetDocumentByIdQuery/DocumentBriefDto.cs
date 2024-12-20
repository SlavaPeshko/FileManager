using Application.Common.Models;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public record DocumentBriefDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public DocumentType Type { get; init; }
    public string Path { get; init; }
    public DateTimeOffset UploadAt { get; init; }
    public int DownloadCount { get; init; }
}