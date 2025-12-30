using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler
    (
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper,
    ILogger<GetRestaurantByIdQueryHandler> logger,
    IBlobStorageService blobStorageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving restaurant with Id: {Id}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                        ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        if(restaurant.LogoUrl!=null)
        {
            restaurantDto.LogoSasUrl =blobStorageService.GetBlobUri(restaurant.LogoUrl);
        }
        return restaurantDto;
    }
}
