using Application.Common.Models;

namespace Application.Documents.Queries.GetDocumentsQuery;

public record DocumentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DocumentType Type { get; set; }
    public string? PreviewPath { get; set; }
    public DateTimeOffset UploadAt { get; set; }
    public int DownloadCount { get; set; }
}