using MediatR;

namespace Application.Documents.Commands.AccessSharedLinkCommand;

public record AccessSharedLinkCommand : IRequest<SharedLinkAccessDto>
{
    public required string UniqueKey { get; init; }
}