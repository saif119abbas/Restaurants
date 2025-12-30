using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Users;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommandHandler
    (
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper,
        ILogger<CreateRestaurantCommandHandler> logger,
        IUserContext userContext
    ) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("{UserEmail} [{UserId}] Creating a new restaurant: {@Restaurant}",
            user.Email,
            user.Id,
            request);
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId=user.Id;
       /* if (!restaurantAuthorization.Authorize(restaurant, ResourceOperation.Create))
        {
            throw new ForbidException();
        }*/
        return await restaurantsRepository.Create(restaurant);
    }
}

