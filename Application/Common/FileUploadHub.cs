using Infrastructure.Attributes;
using Microsoft.AspNetCore.SignalR;

namespace Application.Common;

[HeaderAuthorize]
public class FileUploadHub : Hub
{
    public async Task SendProgress(string fileName, int percentage)
    {
        await Clients.All.SendAsync("ReceiveProgress", fileName, percentage);
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}