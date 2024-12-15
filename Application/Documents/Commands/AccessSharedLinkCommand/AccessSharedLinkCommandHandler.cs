using Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Commands.AccessSharedLinkCommand;

public class AccessSharedLinkCommandHandler : IRequestHandler<AccessSharedLinkCommand, SharedLinkAccessDto>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IPathService _pathService;

    public AccessSharedLinkCommandHandler(IFileManagerDbContext fileManagerDbContext,
        IPathService pathService)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _pathService = pathService;
    }

    public async Task<SharedLinkAccessDto> Handle(AccessSharedLinkCommand request, CancellationToken cancellationToken)
    {
        var sharedLink = await _fileManagerDbContext.SharedLinks
            .Include(sl => sl.Document)
            .FirstOrDefaultAsync(sl => sl.UniqueKey == request.UniqueKey, cancellationToken).ConfigureAwait(false);

        if (sharedLink == null)
        {
            throw new NotFoundException("Shared link not found.");
        }

        if (DateTimeOffset.UtcNow > sharedLink.ExpirationDate)
        {
            throw new BadRequestException("Shared link has expired.");
        }

        var documentPath = _pathService.GetUserDocumentsPath(sharedLink.Document.UserId, sharedLink.Document.UniqueName);
        var fileBytes = await File.ReadAllBytesAsync(documentPath, cancellationToken).ConfigureAwait(false);

        sharedLink.Document.DownloadCount += 1;
        await _fileManagerDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new SharedLinkAccessDto
        {
            Content = fileBytes,
            Name = sharedLink.Document.Name
        };
    }
}