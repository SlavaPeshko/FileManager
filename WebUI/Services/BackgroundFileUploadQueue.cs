using System.Threading.Channels;
using WebUI.Models;

namespace WebUI.Services;

public class BackgroundFileUploadQueue : IBackgroundFileUploadQueue
{
    private readonly Channel<FileUpload> _queue = Channel.CreateUnbounded<FileUpload>();

    public async Task EnqueueAsync(FileUpload model)
    {
        await _queue.Writer.WriteAsync(model);
    }

    public async Task<FileUpload> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}