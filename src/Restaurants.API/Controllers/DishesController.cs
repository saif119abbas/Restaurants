using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Applications.Dishes.Commands.CreateDish;
using Restaurants.Applications.Dishes.Commands.DeleteDishes;
using Restaurants.Applications.Dishes.Dtos;
using Restaurants.Applications.Dishes.Qeuries.GetDishByIdForRestaurant;
using Restaurants.Applications.Dishes.Qeuries.GetDishesForRestauramts;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Infastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
[Authorize]
public class DishesController(IMediator mediator): ControllerBase
{

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int id)
    {

        var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(id,restaurantId));
        return Ok(dish);
    }
    [HttpPost]
    public async Task<ActionResult<int>> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId=restaurantId;
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, id }, null);
    }
    [HttpGet]
    [Authorize(Policy =PolicyNames.AtLeast20)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurants([FromRoute] int restaurantId)
    {

       var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAllDishesForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllDishesForRestaurantCommand(restaurantId));
        return NoContent();
    }
}
