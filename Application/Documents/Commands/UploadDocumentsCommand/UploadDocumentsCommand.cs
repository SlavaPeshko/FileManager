using MediatR;

namespace Application.Documents.Commands.UploadDocumentsCommand;

public record UploadDocumentsCommand : IRequest<IEnumerable<int>>
{
    public required List<Document> Documents { get; init; } = [];
    public required int UserId { get; init; }
    public required string ConnectionId { get; init; }
}

public record Document
{
    public required string Name { get; init; }
    public required byte[] Content { get; init; }
}