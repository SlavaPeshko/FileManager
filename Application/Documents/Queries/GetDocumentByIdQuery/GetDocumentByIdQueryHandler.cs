using Application.Common.Exceptions;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentBriefDto>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IPathService _pathService;

    public GetDocumentByIdQueryHandler(IFileManagerDbContext fileManagerDbContext,
        IPathService pathService)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _pathService = pathService;
    }

    public async Task<DocumentBriefDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _fileManagerDbContext.Documents
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).ConfigureAwait(false);

        if (document == null)
        {
            throw new NotFoundException($"Not found file with id: {request.Id}");
        }

        var documentPath = _pathService.GetUserDocumentsRelativePath(document.User.Id, document.UniqueName);

        return new DocumentBriefDto
        {
            Id = document.Id,
            Name = document.Name,
            DownloadCount = document.DownloadCount,
            Path = documentPath,
            Type = (DocumentType)document.Type,
            UploadAt = document.UploadAt
        };
    }
}