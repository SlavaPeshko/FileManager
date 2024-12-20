using Application.Common.Models;
using MediatR;

namespace Application.Documents.Queries.GetDocumentsQuery;

public record GetDocumentsQuery : IRequest<PaginatedList<DocumentDto>>
{
    public int UserId { get; init; }
}