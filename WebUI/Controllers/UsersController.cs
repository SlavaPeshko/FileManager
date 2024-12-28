using Application.Users.Queries.GetUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var userId = await _mediator.Send(new GetUserQuery
        {
            Name = model.Name,
            Password = model.Password
        });

        return Ok(userId);
    }
}