namespace Infrastructure.Interfaces;

public interface IPathService
{
    string GetWebRootPath();
    string GetDocumentsRootPath();
    string GetUserDocumentsRelativePath(int userId, string documentName);
    string GetUserDocumentsPath(int userId, string documentName);
}