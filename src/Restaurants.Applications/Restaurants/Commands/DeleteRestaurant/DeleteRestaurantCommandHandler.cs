using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler
    (
        IRestaurantsRepository restaurantsRepository,
        ILogger<DeleteRestaurantCommandHandler> logger,
        IRestaurantAuthorizationService restaurantAuthorization
    ) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with Id: {Id}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)??
                         throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        if(!restaurantAuthorization.Authorize(restaurant,ResourceOperation.Delete))
        {
            throw new ForbidException();
        }
        await restaurantsRepository.Delete(restaurant);
    }
}
