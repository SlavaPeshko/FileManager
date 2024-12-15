using MediatR;

namespace Application.Documents.Commands.UploadDocumentCommand;

public record UploadDocumentCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required byte[] Content { get; init; }
    public required int UserId { get; init; }
}