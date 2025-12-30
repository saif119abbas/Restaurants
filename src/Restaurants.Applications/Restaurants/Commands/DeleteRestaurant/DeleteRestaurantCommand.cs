using MediatR;

namespace Restaurants.Applications.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand(int id):IRequest
{
    public int Id { get; } = id;
}
