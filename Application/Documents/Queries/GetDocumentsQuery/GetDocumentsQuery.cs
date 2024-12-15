using Application.Common.Models;
using MediatR;

namespace Application.Documents.Queries.GetDocumentsQuery;

public class GetDocumentsQuery : IRequest<PaginatedList<DocumentDto>>
{
    public required int UserId { get; set; }
}