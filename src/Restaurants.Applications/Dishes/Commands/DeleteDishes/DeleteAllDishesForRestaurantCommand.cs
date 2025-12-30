using MediatR;

namespace Restaurants.Applications.Dishes.Commands.DeleteDishes;

public class DeleteAllDishesForRestaurantCommand(int restaurantId):IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
