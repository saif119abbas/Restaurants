using FluentValidation;
namespace Restaurants.Applications.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .Length(3, 100)
            .WithMessage("Restaurant name must be at least 3 characters, and must not exceed 100 characters.");

        RuleFor(r => r.Category)
            .Must(category => validCategories.Contains(category))
            .WithMessage($"Category must be one of the following: {string.Join(" | ", validCategories)}.");

        RuleFor(r => r.ContactEmail)
            .EmailAddress()
            .When(r => !string.IsNullOrEmpty(r.ContactEmail))
            .WithMessage("Please provide a valid email address.");

        RuleFor(r => r.ContactNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(r => !string.IsNullOrEmpty(r.ContactNumber))
            .WithMessage("Please provide a valid contact number.");

        RuleFor(r => r.City)
            .MaximumLength(50)
            .WithMessage("City must not exceed 50 characters.");

        RuleFor(r => r.Street)
            .MaximumLength(100)
            .WithMessage("Street must not exceed 100 characters.");

        RuleFor(r => r.PostalCode)
            .Matches(@"^\d{2}(-\d{3})?$")
            .WithMessage("Postal code must be in a valid format (e.g., 12-345).");
    }
}

