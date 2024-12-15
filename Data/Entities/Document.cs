using Data.Entities.Base;
using Data.Entities.Enum;

namespace Data.Entities;

public class Document : BaseEntity<int>
{
    public Document()
    {
        SharedLinks = new HashSet<SharedLink>();
    }

    public required string Name { get; set; }
    public DocumentType Type { get; set; }
    public required string UniqueName { get; set; }
    public DateTimeOffset UploadAt { get; set; }
    public int DownloadCount { get; set; }
    public string? PreviewName { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }

    public ICollection<SharedLink> SharedLinks { get; set; }
}