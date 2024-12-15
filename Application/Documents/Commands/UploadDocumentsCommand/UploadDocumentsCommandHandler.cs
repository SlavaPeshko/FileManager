using System.Drawing;
using System.Drawing.Imaging;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PDFiumSharp;
using DocumentEntity = Data.Entities.Document;

namespace Application.Documents.Commands.UploadDocumentsCommand;

public class UploadDocumentsCommandHandler : IRequestHandler<UploadDocumentsCommand, IEnumerable<int>>
{
    private const int CropSize = 50;
    private const int MaxWidth = 50;
    private const int MaxHeight = 50;

    private readonly IFileManagerDbContext _fileManagerDbContext;
    private readonly IPathService _pathService;

    public UploadDocumentsCommandHandler(IFileManagerDbContext fileManagerDbContext,
        IPathService pathService)
    {
        _fileManagerDbContext = fileManagerDbContext;
        _pathService = pathService;
    }

    public async Task<IEnumerable<int>> Handle(UploadDocumentsCommand request, CancellationToken cancellationToken)
    {
        var unsupportedFileNames = request.Documents
            .Where(x => DocumentTypeHelper.GetDocumentType(x.Name) == DocumentType.None)
            .Select(x => x.Name).ToList();

        if (unsupportedFileNames.Any())
        {
            throw new UnsupportedFileTypeException($"Unsupported file type for files: {String.Join(", ", unsupportedFileNames)}");
        }


        var user = await _fileManagerDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            throw new NotFoundException($"Not found user with given Id: {request.UserId}");
        }

        var userPath = Path.Combine(_pathService.GetDocumentsRootPath(), request.UserId.ToString());
        DirectoryHelper.EnsureDirectoryExists(userPath);

        List<DocumentEntity> documents = [];
        List<string> documentPaths = [];
        foreach (var document in request.Documents)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{document.Name}";
            documentPaths.Add(Path.Combine(userPath, uniqueFileName));

            string? previewFileName = null;
            var documentType = DocumentTypeHelper.GetDocumentType(document.Name);
            if (documentType == DocumentType.Pdf || documentType == DocumentType.Picture)
            {
                previewFileName = GeneratePreview(document.Content, document.Name, userPath, documentType);
            }

            documents.Add(new DocumentEntity
            {
                Name = document.Name,
                UniqueName = uniqueFileName,
                UploadAt = DateTimeOffset.UtcNow,
                Type = (Data.Entities.Enum.DocumentType)DocumentTypeHelper.GetDocumentType(document.Name),
                User = user,
                PreviewName = previewFileName,
            });
        }

        await using var transaction = await _fileManagerDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var writeTasks = request.Documents.Select((doc, index) =>
                File.WriteAllBytesAsync(documentPaths[index], doc.Content, cancellationToken)
            );
            await Task.WhenAll(writeTasks);

            await _fileManagerDbContext.Documents.AddRangeAsync(documents, cancellationToken);
            await _fileManagerDbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var path in documentPaths.Where(File.Exists))
            {
                File.Delete(path);
            }

            throw;
        }

        return documents.Select(x => x.Id);
    }

    private string? GeneratePreview(byte[] fileContent, string fileName, string previewDirectory, DocumentType documentType)
    {
        try
        {
            var previewFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_preview.jpg";
            var previewPath = Path.Combine(previewDirectory, previewFileName);

            switch (documentType)
            {
                case DocumentType.Pdf:
                    File.WriteAllBytes(previewPath, ConvertPdfToImage(fileContent));
                    break;

                case DocumentType.Picture:
                    File.WriteAllBytes(previewPath, CreatePreviewImage(fileContent));
                    break;

                default:
                    return null;
            }

            return previewFileName;
        }
        catch
        {
            return null;
        }
    }

    private static byte[] CreatePreviewImage(byte[] imageContent)
    {
        using var inputStream = new MemoryStream(imageContent);
        using var originalImage = Image.FromStream(inputStream);

        var aspectRatio = (double)originalImage.Width / originalImage.Height;
        int newWidth, newHeight;

        if (aspectRatio > 1)
        {
            newWidth = MaxWidth;
            newHeight = (int)(MaxWidth / aspectRatio);
        }
        else
        {
            newWidth = (int)(MaxHeight * aspectRatio);
            newHeight = MaxHeight;
        }

        using var previewImage = new Bitmap(newWidth, newHeight);

        using (var graphics = Graphics.FromImage(previewImage))
        {
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
        }

        using var outputStream = new MemoryStream();
        previewImage.Save(outputStream, ImageFormat.Jpeg);

        return outputStream.ToArray();
    }


    public static byte[] ConvertPdfToImage(byte[] fileContent)
    {
        using var document = new PdfDocument(fileContent);
        var pdfPage = document.Pages[1];
        using var image = new PDFiumBitmap(CropSize, CropSize, true);
        pdfPage?.Render(image);

        using MemoryStream memoryStreamBMP = new();
        image.Save(memoryStreamBMP);

        using var imageBmp = Image.FromStream(memoryStreamBMP);

        using MemoryStream memoryStreamJpg = new();
        imageBmp.Save(memoryStreamJpg, ImageFormat.Jpeg);

        return memoryStreamJpg.ToArray();
    }
}