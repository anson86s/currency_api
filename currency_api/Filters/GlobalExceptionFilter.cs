using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NotFoundException = currency_api.Exceptions.NotFoundException;
using UnauthorizedAccessException = currency_api.Exceptions.UnauthorizedAccessException;
using ValidationException = currency_api.Exceptions.ValidationException;
using currency_api.Models;

namespace currency_api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,

            ValidationException => StatusCodes.Status400BadRequest,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            _ => StatusCodes.Status500InternalServerError
        };

        var result = new Result
        {
            IsSuccess = false,
            ReturnCode = statusCode.ToString(),
            ReturnMessage = context.Exception.StackTrace ?? context.Exception.Message
        };

        context.Result = new ObjectResult(result)
        {
            StatusCode = statusCode
        };
    }
}

