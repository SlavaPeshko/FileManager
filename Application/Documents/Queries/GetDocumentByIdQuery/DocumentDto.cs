using Application.Common.Models;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public class DocumentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DocumentType Type { get; set; }
    public required string Path { get; set; }
    public DateTimeOffset UploadAt { get; set; }
    public int DownloadCount { get; set; }
}