using Data.Entities.Base;
using Data.Entities.Enum;

namespace Data.Entities;

public class Document : BaseEntity<int>
{
    public required string Name { get; set; }
    public DocumentType Type { get; set; }
    public required string Path { get; set; }
    public DateTimeOffset UploadAt { get; set; }
    public int DownloadCount { get; set; }
}