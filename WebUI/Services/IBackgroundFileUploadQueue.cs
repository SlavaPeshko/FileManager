using WebUI.Models;

namespace WebUI.Services;

public interface IBackgroundFileUploadQueue
{
    Task EnqueueAsync(FileUpload model);
    Task<FileUpload> DequeueAsync(CancellationToken cancellationToken);
}