using MediatR;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public record GetDocumentByIdQuery : IRequest<DocumentBriefDto>
{
    public required int Id { get; set; }
}