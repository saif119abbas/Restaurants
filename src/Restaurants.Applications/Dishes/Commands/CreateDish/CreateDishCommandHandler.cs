using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler
    (
        ILogger<CreateDishCommandHandler> logger,
        IDishesRepository dishesRepository,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper,
        IRestaurantAuthorizationService restaurantAuthorization
    ) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new dish: {@DishRequest}",request);
        var restaurant=await  restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
            throw new NotFoundException(nameof(Restaurant),request.RestaurantId.ToString());
        if (!restaurantAuthorization.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbidException();
        }
        var dish = mapper.Map<Dish>(request);
        restaurant.Dishes.Add(dish);
        return await dishesRepository.Create(dish);
    }
}
