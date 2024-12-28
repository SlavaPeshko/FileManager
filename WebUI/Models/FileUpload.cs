namespace WebUI.Models;

public record FileUpload
{
    public required string FileName { get; init; }
    public required byte[] Content { get; init; }
    public required int UserId { get; init; }
    public required string ConnectionId { get; init; }
}