using MediatR;
using Restaurants.Applications.Dishes.Dtos;

namespace Restaurants.Applications.Dishes.Qeuries.GetDishesForRestauramts;

public class GetDishesForRestaurantQuery(int rstaurantId):IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = rstaurantId;
}
