using Application.Common.Models;

namespace Application.Common.Helpers;

public static class DocumentTypeHelper
{
    public static DocumentType GetDocumentType(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return DocumentType.None;

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        return extension switch
        {
            ".pdf" => DocumentType.Pdf,
            ".xls" or ".xlsx" => DocumentType.Excel,
            ".doc" or ".docx" => DocumentType.Word,
            ".txt" => DocumentType.Txt,
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => DocumentType.Picture,
            _ => DocumentType.None
        };
    }
}