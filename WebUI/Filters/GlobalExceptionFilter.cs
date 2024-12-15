using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = 500;
        var response = new
        {
            message = "An unexpected error occurred.",
            details = context.Exception.Message
        };

        switch (context.Exception)
        {
            case NotFoundException:
                statusCode = 404;
                response = new
                {
                    message = context.Exception.Message,
                    details = "The requested resource could not be found."
                };
                break;
            case UnsupportedFileTypeException:
                statusCode = 415;
                response = new
                {
                    message = context.Exception.Message,
                    details = "The file type provided is not supported."
                };
                break;
            case BadRequestException:
                statusCode = 400;
                response = new
                {
                    message = context.Exception.Message,
                    details = "The request could not be processed due to invalid input."
                };
                break;
        }

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}