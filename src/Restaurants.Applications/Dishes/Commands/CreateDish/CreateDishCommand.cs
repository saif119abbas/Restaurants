using MediatR;

namespace Restaurants.Applications.Dishes.Commands.CreateDish;

public class CreateDishCommand:IRequest<int>
{
    public int RestaurantId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
}
