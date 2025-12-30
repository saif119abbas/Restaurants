using MediatR;
using Restaurants.Applications.Dishes.Dtos;

namespace Restaurants.Applications.Dishes.Qeuries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int id,int restauranrId):IRequest<DishDto>
{
    public int Id { get; } = id;
    public int RestaurantId { get; } = restauranrId;
}
