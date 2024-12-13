using MediatR;

namespace Application.Users.Queries.GetUserQuery;

public record GetUserQuery : IRequest<int>
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}