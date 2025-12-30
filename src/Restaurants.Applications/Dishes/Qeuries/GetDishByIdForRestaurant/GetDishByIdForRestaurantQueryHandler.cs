using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Dishes.Qeuries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQueryHandler(
    ILogger<GetDishByIdForRestaurantQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish {DishId} for restaurant {RestaurantId}",
            request.Id, request.RestaurantId);
        var restaurant=await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dish = restaurant.Dishes.FirstOrDefault(d=>d.Id==request.Id);
        if(dish==null)
        {
            throw new NotFoundException(nameof(Dish), request.Id.ToString());
        }
        var dishDto=mapper.Map<DishDto>(dish);
        return dishDto;
    }
}
