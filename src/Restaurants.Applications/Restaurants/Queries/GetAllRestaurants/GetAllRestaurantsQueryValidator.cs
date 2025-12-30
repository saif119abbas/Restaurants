using FluentValidation;
using Restaurants.Applications.Restaurants.Dtos;

namespace Restaurants.Applications.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator:AbstractValidator<GetAllRestaurantsQuery>
    {
        private int [] allowPageSizes = [5, 10, 15, 30];
        private string[] allowedSortByColumnNames =
            [
                nameof(RestaurantDto.Name),
                nameof(RestaurantDto.Category),
                nameof(RestaurantDto.Description),
            ];
        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize)
               .Must(value=> allowPageSizes.Contains(value))
               .WithMessage($"The page size must be in [{string.Join(",", allowPageSizes)}]");
            RuleFor(r => r.SortBy)
             .Must(value => allowedSortByColumnNames.Contains(value))
             .When(q => q.SortBy != null)
             .WithMessage($"The sort by is optinal, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
