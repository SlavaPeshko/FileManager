using System.Net;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HeaderAuthorize : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(metadata => metadata is AllowAnonymousAttribute);

        if (hasAllowAnonymous)
        {
            return;
        }

        var userId = context.HttpContext.Request.GetUserIdFromHeader();

        if (userId == 0)
        {
            SetErrorResponse(context, HttpStatusCode.BadRequest, "Missing User Id", "Please provide a valid User Id.");
            return;
        }

        var fileManagerDbContext = context.HttpContext.RequestServices.GetService<IFileManagerDbContext>();

        if (fileManagerDbContext == null)
        {
            SetErrorResponse(context, HttpStatusCode.InternalServerError, "Database Error", "Database service is unavailable.");
            return;
        }

        if (!fileManagerDbContext.Users.Any(x => x.Id == userId))
        {
            SetErrorResponse(context, HttpStatusCode.Unauthorized, "Invalid User Id", "The provided User Id does not exist.");
        }
    }

    private void SetErrorResponse(AuthorizationFilterContext context, HttpStatusCode statusCode, string reasonPhrase, string message)
    {
        context.HttpContext.Response.StatusCode = (int)statusCode;
        context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = reasonPhrase;

        context.Result = new JsonResult(new
        {
            Status = statusCode.ToString(),
            Message = message
        });
    }
}