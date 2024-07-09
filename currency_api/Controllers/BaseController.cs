
using currency_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace currency_api.Controllers;

public class BaseController<T> : ControllerBase where T : BaseController<T>
{
    protected OkObjectResult Success()
    {
        return Ok(new Result { IsSuccess = true });
    }

    protected ActionResult<Result<TData>> Success<TData>(TData data) where TData : class
    {
        return Ok(new Result<TData> { IsSuccess = true, Data = data });
    }

    protected OkObjectResult Fail(string returnCode, string returnMessage)
    {
        return Ok(new Result
        {
            IsSuccess = false,
            ReturnCode = returnCode,
            ReturnMessage = returnMessage
        });
    }
}

