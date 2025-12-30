using MediatR;
using Restaurants.Applications.Common;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Domain.Constatnts;

namespace Restaurants.Applications.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string ? SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber{ get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

}
