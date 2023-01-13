using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MangaHunter.API.Controllers;

public class ErrorsController : ControllerBase
{
    [HttpGet("/error")]
    public IActionResult Error()
    {
        var ex = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem();
    }
}