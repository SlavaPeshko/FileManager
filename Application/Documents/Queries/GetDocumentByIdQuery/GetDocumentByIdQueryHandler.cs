using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;

    public GetDocumentByIdQueryHandler(IFileManagerDbContext fileManagerDbContext)
    {
        _fileManagerDbContext = fileManagerDbContext;
    }

    public async Task<DocumentDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _fileManagerDbContext.Documents
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).ConfigureAwait(false);

        if (document == null)
        {
            throw new NotFoundException($"Not found file with id: {request.Id}");
        }

        return new DocumentDto
        {
            Id = document.Id,
            Name = document.Name,
            DownloadCount = document.DownloadCount,
            Path = document.UniqueName,
            Type = (DocumentType)document.Type,
            UploadAt = document.UploadAt
        };
    }
}