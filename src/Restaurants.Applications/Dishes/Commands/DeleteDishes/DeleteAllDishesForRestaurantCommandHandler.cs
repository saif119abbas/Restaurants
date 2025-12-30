using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Dishes.Commands.DeleteDishes;

public class DeleteAllDishesForRestaurantCommandHandler
    (
        ILogger<DeleteAllDishesForRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorization
    ) : IRequestHandler<DeleteAllDishesForRestaurantCommand>
{
    public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dishes for restaurant {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        if (!restaurantAuthorization.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbidException();
        }
        await dishesRepository.Delete(restaurant.Dishes);
    }
}
