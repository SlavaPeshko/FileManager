using Application.Documents.Commands.UploadDocumentCommand;
using Application.Documents.Queries.GetDocumentByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDocuments()
    {
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDocument(int id)
    {
        var document = await _mediator.Send(new GetDocumentByIdQuery{ Id = id });

        return Ok(document);
    }

    [HttpPost]
    public async Task<IActionResult> UploadDocument([FromForm]int userId, [FromForm] IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var fileContents = stream.ToArray();

        var documentId = await _mediator.Send(new UploadDocumentCommand
        {
            Name = file.FileName,
            Content = fileContents,
            UserId = userId
        });

        return Ok(documentId);
    }

    [HttpPost("upload-multiple")]
    public async Task<IActionResult> UploadDocuments([FromForm] IFormFile[] files)
    {
        return Ok();
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        return File(string.Empty, "application/octet-stream");
    }

    [HttpPost("share/{id}")]
    public async Task<IActionResult> ShareDocument(int id, [FromQuery] TimeSpan expiry)
    {
        return Ok(string.Empty);
    }
}
