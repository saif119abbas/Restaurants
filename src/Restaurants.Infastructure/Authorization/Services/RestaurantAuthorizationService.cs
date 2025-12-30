using Microsoft.Extensions.Logging;
using Restaurants.Applications.Users;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infastructure.Authorization.Services;

public class RestaurantAuthorizationService
    (
        ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext
    ): IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation )
    {
        var user = userContext.GetCurrentUser();
        if(user == null)
        {
            throw new UnauthorizedException();
        }
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email,
            resourceOperation,
            restaurant.Name
            );
        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read Operation - successful authorization");
            return true;
        }
        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete Operation - successful authorization");
            return true;
        }
        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) &&
            user.Id==restaurant.OwnerId
            )
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }
        return false;
    }
}
