using MediatR;

namespace Application.Documents.Queries.GetDocumentByIdQuery;

public record GetDocumentByIdQuery : IRequest<DocumentDto>
{
    public int Id { get; set; }
}