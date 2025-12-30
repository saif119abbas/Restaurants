using FluentValidation;
namespace Restaurants.Applications.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator: AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .Length(3, 100)
            .WithMessage("Restaurant name must be at least 3 characters, and must not exceed 100 characters.");
    }
}
