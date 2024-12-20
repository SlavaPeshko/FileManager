using MediatR;

namespace Application.Users.Queries.GetUserQuery;

public record GetUserQuery : IRequest<int>
{
    public required string Name { get; init; }
    public required string Password { get; init; }
}