using Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserQuery;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, int>
{
    private readonly IFileManagerDbContext _fileManagerDbContext;

    public GetUserQueryHandler(IFileManagerDbContext fileManagerDbContext)
    {
        _fileManagerDbContext = fileManagerDbContext;
    }

    public async Task<int> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = await _fileManagerDbContext.Users
            .Where(x => x.Name == request.Name && x.Password == request.Password)
            .Select(x => x.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (userId == 0)
        {
            throw new NotFoundException($"Not found user with name: {request.Name}");
        }

        return userId;
    }
}