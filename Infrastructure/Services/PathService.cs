using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class PathService : IPathService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    private readonly string _fileManagerPath;

    public PathService(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;

        _fileManagerPath = configuration.GetSection("FileManager:Path").Value ??
                           throw new ArgumentNullException("Directory is not configured.");
    }

    public string GetWebRootPath()
    {
        return _hostEnvironment.WebRootPath ?? throw new InvalidOperationException("WebRootPath is not configured.");
    }

    public string GetDocumentsRootPath()
    {
        return Path.Combine(GetWebRootPath(), _fileManagerPath);
    }

    public string GetUserDocumentsRelativePath(int userId, string documentName)
    {
        if (userId <= 0)
        {
            throw new ArgumentException($"Invalid user id: {userId}. User id must be greater than 0.", nameof(userId));
        }

        if (string.IsNullOrEmpty(documentName))
        {
            throw new ArgumentNullException(nameof(documentName), "Document name cannot be null or empty.");
        }

        return Path.Combine(_fileManagerPath, userId.ToString(), documentName);
    }
    
    public string GetUserDocumentsPath(int userId, string documentName)
    {
        return Path.Combine(GetWebRootPath(), GetUserDocumentsRelativePath(userId, documentName));
    }
}