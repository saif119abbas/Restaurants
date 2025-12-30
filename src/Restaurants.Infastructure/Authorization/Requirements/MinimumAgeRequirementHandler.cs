using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Applications.Users;

namespace Restaurants.Infastructure.Authorization.Requirements;

internal class MinimumAgeRequirementHandler
    (
        ILogger<MinimumAgeRequirementHandler> logger,
        IUserContext userContext
    )
    : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoF}- Handling MinimumAgeRequirement",
            currentUser?.Email,
            currentUser?.DateOfBirth
            );
        if(currentUser?.DateOfBirth ==null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        if(currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge)<= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization Succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}
