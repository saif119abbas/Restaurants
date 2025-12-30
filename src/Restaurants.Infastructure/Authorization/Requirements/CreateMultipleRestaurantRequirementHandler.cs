using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infastructure.Authorization.Requirements;

public class CreateMultipleRestaurantRequirementHandler
    (
        ILogger<CreateMultipleRestaurantRequirementHandler> logger,
        IUserContext userContext,
        IRestaurantsRepository restaurantsRepository
    )
    : AuthorizationHandler<CreateMultipleRestaurantRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        CreateMultipleRestaurantRequirement requirement)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoB}- Handling CreateMultipleRestaurantRequirement",
           user?.Email,
           user?.DateOfBirth
           );
        var restaurants = await restaurantsRepository.GetAllAsync();
        var userRestaurantCreated=restaurants.Count(r=>r.OwnerId==user?.Id);
        logger.LogInformation("The owner has {userRestaurantCreated} restaurants", userRestaurantCreated);
        if (userRestaurantCreated >= requirement.MinumRestaurantCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
