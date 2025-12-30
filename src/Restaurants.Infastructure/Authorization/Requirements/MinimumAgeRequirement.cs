using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infastructure.Authorization.Requirements;

public class MinimumAgeRequirement(int minimumAge):IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
