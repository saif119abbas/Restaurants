using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler
    (
        IMapper mapper, 
        IRestaurantsRepository restaurantsRepository,
        ILogger<UpdateRestaurantCommandHandler> logger,
        IRestaurantAuthorizationService restaurantAuthorization
    ) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with Id: {Id} {@UpdateRestaurant}", request.Id,request);
        var restaurant=await restaurantsRepository.GetByIdAsync(request.Id)
                       ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        if (!restaurantAuthorization.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbidException();
        }
        mapper.Map(request, restaurant);
        await restaurantsRepository.SaveChanges();
    }
}
