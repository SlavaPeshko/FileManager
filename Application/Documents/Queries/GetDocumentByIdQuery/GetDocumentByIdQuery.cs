using MediatR;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public record GetDocumentByIdQuery : IRequest<DocumentBriefDto>
{
    public int Id { get; init; }
}