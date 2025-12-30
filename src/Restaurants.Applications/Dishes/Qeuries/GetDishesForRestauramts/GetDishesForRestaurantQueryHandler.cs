using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Dishes.Qeuries.GetDishesForRestauramts;

public class GetDishesForRestaurantQueryHandler(
    ILogger<GetDishesForRestaurantQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository
    ) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dishes for restaurant {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if(restaurant == null )
        {
            throw new NotFoundException(nameof(Restaurant),request.RestaurantId.ToString());
        }
        var dishes = restaurant.Dishes;
        var dishDtos=mapper.Map<IEnumerable<DishDto>>(dishes);
        return dishDtos;
   
    }
}
