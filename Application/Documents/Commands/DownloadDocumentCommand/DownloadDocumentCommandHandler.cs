using Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Commands.DownloadDocumentCommand;

public class DownloadDocumentCommandHandler : IRequestHandler<DownloadDocumentCommand, DownloadDocumentDto>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IPathService _pathService;

    public DownloadDocumentCommandHandler(IFileManagerDbContext fileManagerDbContext,
        IPathService pathService)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _pathService = pathService;
    }

    public async Task<DownloadDocumentDto> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _fileManagerDbContext.Documents
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).ConfigureAwait(false);

        if (document == null)
        {
            throw new NotFoundException($"Not found file with id: {request.Id}");
        }

        var documentPath = _pathService.GetUserDocumentsPath(document.UserId, document.UniqueName);
        var fileBytes = await File.ReadAllBytesAsync(documentPath, cancellationToken).ConfigureAwait(false);

        document.DownloadCount += 1;
        await _fileManagerDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new DownloadDocumentDto
        {
            Content = fileBytes,
            Name = document.Name
        };
    }
}