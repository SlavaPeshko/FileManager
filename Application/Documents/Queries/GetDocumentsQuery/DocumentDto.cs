using Application.Common.Models;

namespace Application.Documents.Queries.GetDocumentsQuery;

public record DocumentDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public DocumentType Type { get; init; }
    public string? PreviewPath { get; init; }
    public DateTimeOffset UploadAt { get; init; }
    public int DownloadCount { get; init; }
}