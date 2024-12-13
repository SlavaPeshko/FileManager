using MediatR;

namespace Application.Documents.Commands.UploadDocumentCommand;

public record UploadDocumentCommand : IRequest<int>
{
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
    public required int UserId { get; set; }
}