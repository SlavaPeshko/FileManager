using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions;

public static class HttpRequestExtension
{
    public static int GetUserIdFromHeader(this HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!request.Headers.TryGetValue("User-Id", out var values))
        {
            return default;
        }

        return int.TryParse(values.FirstOrDefault(), out var userId) ? userId : default;
    }
}