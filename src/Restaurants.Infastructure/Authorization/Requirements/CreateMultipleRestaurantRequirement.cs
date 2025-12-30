using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infastructure.Authorization.Requirements;

public class CreateMultipleRestaurantRequirement(int minumRestaurantCreated):IAuthorizationRequirement
{
    public int MinumRestaurantCreated { get; } = minumRestaurantCreated;
}
