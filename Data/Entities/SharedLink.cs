using Data.Entities.Base;

namespace Data.Entities;

public class SharedLink : BaseEntity<int>
{
    public required string UniqueKey { get; init; }
    public DateTimeOffset ExpirationDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public int DocumentId { get; init; }
    public required Document Document { get; init; }
}