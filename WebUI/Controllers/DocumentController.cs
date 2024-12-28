using Application.Documents.Commands.AccessSharedLinkCommand;
using Application.Documents.Commands.CreateSharedLinkCommand;
using Application.Documents.Commands.DownloadDocumentCommand;
using Application.Documents.Commands.UploadDocumentCommand;
using Application.Documents.Queries.GetDocumentByIdQuery;
using Application.Documents.Queries.GetDocumentsQuery;
using Infrastructure.Attributes;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;
using WebUI.ViewModels;

namespace WebUI.Controllers;

[HeaderAuthorize]
[ApiController]
[Route("[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBackgroundFileUploadQueue _fileUploadQueue;

    public DocumentsController(IMediator mediator, IBackgroundFileUploadQueue fileUploadQueue)
    {
        _mediator = mediator;
        _fileUploadQueue = fileUploadQueue;
    }

    [HttpGet]
    public async Task<IActionResult> GetDocuments()
    {
        var documents = await _mediator.Send(new GetDocumentsQuery
        {
            UserId = Request.GetUserIdFromHeader()
        });

        return Ok(documents);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDocument(int id)
    {
        var document = await _mediator.Send(new GetDocumentByIdQuery{ Id = id });

        return Ok(document);
    }

    [HttpPost]
    public async Task<IActionResult> UploadDocument([FromForm] IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var fileContents = stream.ToArray();

        var documentId = await _mediator.Send(new UploadDocumentCommand
        {
            Name = file.FileName,
            Content = fileContents,
            UserId = Request.GetUserIdFromHeader()
        });

        return Ok(documentId);
    }

    [HttpPost("upload-multiple")]
    public async Task<IActionResult> UploadDocuments([FromForm] IFormFile[] files, [FromForm] string connectionId)
    {
        if (files == null || files.Length == 0)
        {
            return BadRequest("No files were uploaded.");
        }

        foreach (var file in files)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            await _fileUploadQueue.EnqueueAsync(new FileUpload
            {
                FileName = file.FileName,
                Content = stream.ToArray(),
                UserId = Request.GetUserIdFromHeader(),
                ConnectionId = connectionId
            });
        }

        return Ok();
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        var document = await _mediator.Send(new DownloadDocumentCommand{ Id = id });

        return File(document.Content, "application/octet-stream", document.Name);
    }

    [HttpPost("{id:int}/share")]
    public async Task<IActionResult> CreateShareDocument([FromBody]SharedLinkModel model, int id)
    {
        var createSharedLinkDto = await _mediator.Send(new CreateSharedLinkCommand
        {
            Id = id,
            DurationInSeconds = model.DurationInSeconds
        });

        return Ok(new
        {
            sharedLink = $"{Request.Scheme}://{Request.Host}/documents/shared/{createSharedLinkDto.UniqueKey}",
            expirationDate = createSharedLinkDto.ExpirationDate
        });
    }

    [AllowAnonymous]
    [HttpGet("shared/{uniqueKey}")]
    public async Task<IActionResult> AccessSharedLink(string uniqueKey)
    {
        var document = await _mediator.Send(new AccessSharedLinkCommand
        {
            UniqueKey = uniqueKey
        });

        return File(document.Content, "application/octet-stream", document.Name);
    }
}
