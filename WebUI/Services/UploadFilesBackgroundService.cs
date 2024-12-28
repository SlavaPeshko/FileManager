using Application.Documents.Commands.UploadDocumentsCommand;
using MediatR;

namespace WebUI.Services;

public class UploadFilesBackgroundService : BackgroundService
{
    private readonly IBackgroundFileUploadQueue _backgroundFileUploadQueue;
    private readonly IServiceScopeFactory _scopeFactory;

    public UploadFilesBackgroundService(IBackgroundFileUploadQueue backgroundFileUploadQueue,
        IServiceScopeFactory scopeFactory)
    {
        _backgroundFileUploadQueue = backgroundFileUploadQueue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var fileUpload = await _backgroundFileUploadQueue.DequeueAsync(stoppingToken);

            await mediator.Send(new UploadDocumentsCommand
            {
                Documents =
                [
                    new()
                    {
                        Name = fileUpload.FileName,
                        Content = fileUpload.Content,
                    }
                ],
                UserId = fileUpload.UserId,
                ConnectionId = fileUpload.ConnectionId
            }, stoppingToken);
        }
    }
}