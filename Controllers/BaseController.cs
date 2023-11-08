using Microsoft.AspNetCore.Mvc;
using Web.ControllerApi.Template.Models;

namespace Web.ControllerApi.Template.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult ToActionResult<T>(IApiResponse<T> apiResponse)
    {
        return StatusCode(apiResponse.Code, apiResponse);
    }
}