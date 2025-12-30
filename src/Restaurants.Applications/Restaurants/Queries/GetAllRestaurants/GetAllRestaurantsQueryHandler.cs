
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Common;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler
    (
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper,
        ILogger<GetAllRestaurantsQueryHandler> logger
    ) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all restaurants @{Request}",request);
        var (restaurants,totalCount) =  await restaurantsRepository
                                .GetAllMatchingAsync(request.SearchPhrase,request.PageNumber,request.PageSize,request.SortBy,request.SortDirection);
        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        var result=new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, request.PageSize,request.PageNumber);
        return result;
    }
}
