using MediatR;

namespace Application.Documents.Commands.DownloadDocumentCommand;

public record DownloadDocumentCommand : IRequest<DownloadDocumentDto>
{
    public required int Id { get; init; }
}