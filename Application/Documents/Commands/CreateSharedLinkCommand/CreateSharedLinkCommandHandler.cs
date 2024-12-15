using Application.Common.Exceptions;
using Data.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Commands.CreateSharedLinkCommand;

public class CreateSharedLinkCommandHandler : IRequestHandler<CreateSharedLinkCommand, string>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;

    public CreateSharedLinkCommandHandler(IFileManagerDbContext fileManagerDbContext)
    {
        _fileManagerDbContext = fileManagerDbContext;
    }

    public async Task<string> Handle(CreateSharedLinkCommand request, CancellationToken cancellationToken)
    {
        var document = await _fileManagerDbContext.Documents
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).ConfigureAwait(false);

        if (document == null)
        {
            throw new NotFoundException($"Not found file with id: {request.Id}");
        }

        var sharedLink = new SharedLink
        {
            UniqueKey = Guid.NewGuid().ToString(),
            ExpirationDate = DateTimeOffset.Now.AddSeconds(request.DurationInSeconds),
            Document = document,
        };

        _fileManagerDbContext.SharedLinks.Add(sharedLink);
        await _fileManagerDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return sharedLink.UniqueKey;
    }
}