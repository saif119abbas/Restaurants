using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Applications.Restaurants.Commands.CreateRestaurant;
using Restaurants.Applications.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Applications.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Applications.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Applications.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Applications.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constatnts;
using Restaurants.Infastructure.Authorization;

namespace Restaurants.API.Controllers;
[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantController(IMediator mediator) : ControllerBase
{
   
    [HttpGet]
    [AllowAnonymous]
   // [Authorize(Policy =PolicyNames.CreatedAtLeast2Restaurants)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    //[Authorize(Policy =PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute]int id)
    {

        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }
    [HttpPost]
    [Authorize(Roles =UserRoles.Owner)]
    public async Task<ActionResult<int>> Create(CreateRestaurantCommand command)
    {
        int id= await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id)); 
        return NoContent();
    }
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
    [HttpPost("{id}/logo")]
    public async Task<IActionResult> UploadLogo([FromRoute] int id,IFormFile  file)
    {
        using var stream = file.OpenReadStream();
        var command = new UploadRestaurantLogoCommand()
        {
            RestaurantId = id,
            File = stream,
            FileName = $"{id}-{file.FileName}"
        };
        var logoUrl=await mediator.Send(command);
        return Ok(logoUrl);
    }
}
