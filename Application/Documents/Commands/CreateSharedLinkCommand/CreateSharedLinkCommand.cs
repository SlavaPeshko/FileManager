using MediatR;

namespace Application.Documents.Commands.CreateSharedLinkCommand;

public record CreateSharedLinkCommand : IRequest<string>
{
    public required int Id { get; init; }
    public required int DurationInSeconds { get; init; }
}