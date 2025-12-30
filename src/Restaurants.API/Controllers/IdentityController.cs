using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Applications.Users.Commands.UpdateUserDetails;
using Restaurants.Applications.Users.Commands.AssignUserRole;
using Restaurants.Domain.Constatnts;
using Restaurants.Applications.Users.Commands.RemoveUserRole;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) :ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
         await mediator.Send(command);
        return NoContent();
    }
    [HttpPost("userRole")]
    [Authorize(Roles=UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> RemoveUserRole(RemoveUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
