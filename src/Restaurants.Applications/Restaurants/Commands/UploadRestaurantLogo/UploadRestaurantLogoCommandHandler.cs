using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Applications.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler
    (
        ILogger<UploadRestaurantLogoCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorization,
        IBlobStorageService blobStorageService
    )
    : IRequestHandler<UploadRestaurantLogoCommand,string>
{
    public async Task<string> Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading restaurant logo for Id: {Id}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                       ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if (!restaurantAuthorization.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbidException();
        }
        var logoUrl=await blobStorageService.UploadFileAsync(request.File, request.FileName);
        restaurant.LogoUrl = logoUrl;
        await restaurantsRepository.SaveChanges();
        return logoUrl;

    }
}
