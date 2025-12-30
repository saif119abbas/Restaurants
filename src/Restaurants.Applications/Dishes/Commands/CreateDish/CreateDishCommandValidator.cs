using FluentValidation;

namespace Restaurants.Applications.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidator()
        {
            RuleFor(d => d.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The price must be a positive number.");

            RuleFor(d => d.KiloCalories)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The calories must be a positive number.");
        }
    }
}
