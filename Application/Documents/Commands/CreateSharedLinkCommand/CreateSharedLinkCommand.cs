using MediatR;

namespace Application.Documents.Commands.CreateSharedLinkCommand;

public record CreateSharedLinkCommand : IRequest<CreateSharedLinkDto>
{
    public required int Id { get; init; }
    public required int DurationInSeconds { get; init; }
}