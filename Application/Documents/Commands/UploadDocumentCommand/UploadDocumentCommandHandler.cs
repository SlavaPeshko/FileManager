using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Documents.Commands.UploadDocumentCommand;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, int>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IConfiguration _configuration;

    public UploadDocumentCommandHandler(IFileManagerDbContext fileManagerDbContext,
        IConfiguration configuration)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _configuration = configuration;
    }

    public async Task<int> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var fileManagerPath = _configuration.GetSection("FileManager:Path").Value ??
                   throw new ArgumentNullException("Directory is not configured.");

        if (!Directory.Exists(fileManagerPath))
        {
            Directory.CreateDirectory(fileManagerPath);
        }

        var user = await _fileManagerDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            throw new NotFoundException($"Not found user with given Id: {request.UserId}");
        }

        var userPath = Path.Combine(fileManagerPath, request.UserId.ToString());
        if (!Directory.Exists(userPath))
        {
            Directory.CreateDirectory(userPath);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{request.Name}";
        var filePath = Path.Combine(userPath, uniqueFileName);

        await File.WriteAllBytesAsync(filePath, request.Content, cancellationToken);

        var document = new Document
        {
            Name = request.Name,
            UniqueName = filePath,
            UploadAt = DateTimeOffset.UtcNow,
            User = user
        };

        await _fileManagerDbContext.Documents.AddAsync(document, cancellationToken).ConfigureAwait(false);
        await _fileManagerDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return document.Id;
    }
}