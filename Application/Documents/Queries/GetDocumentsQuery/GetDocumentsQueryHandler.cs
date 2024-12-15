using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Queries.GetDocumentsQuery;

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, PaginatedList<DocumentDto>>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IPathService _pathService;

    public GetDocumentsQueryHandler(IFileManagerDbContext fileManagerDbContext, IPathService pathService)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _pathService = pathService;
    }

    public async Task<PaginatedList<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var documents = await _fileManagerDbContext.Documents
            .Where(x => x.User.Id == request.UserId)
            .OrderByDescending(x=> x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var items = new List<DocumentDto>();
        foreach (var document in documents)
        {
            items.Add(new DocumentDto
            {
                Id = document.Id,
                Name = document.Name,
                DownloadCount = document.DownloadCount,
                PreviewPath = document.PreviewName != null
                    ? _pathService.GetUserDocumentsRelativePath(request.UserId, document.PreviewName)
                    : null,
                Type = (DocumentType)document.Type,
                UploadAt = document.UploadAt
            });
        }

        return new PaginatedList<DocumentDto>(items, items.Count, 1, 1);
    }
}