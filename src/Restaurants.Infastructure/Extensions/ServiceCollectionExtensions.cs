using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infastructure.Persistence;
using Restaurants.Infastructure.Seedrs;
using Restaurants.Domain.Repositories;
using Restaurants.Infastructure.Repositories;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Restaurants.Infastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infastructure.Authorization.Requirements;
using Restaurants.Infastructure.Authorization.Services;
using Restaurants.Domain.Interfaces;
using Restaurants.Infastructure.Configuration;
using Restaurants.Infastructure.Storage;

namespace Restaurants.Infastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options=>options.UseSqlServer(connectionString)
        .EnableSensitiveDataLogging()); 
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();
        services.AddScoped<IRestaruntSeeder, RestaruntSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, 
            builder => builder.RequireClaim(AppCalimType.Nationality, "Palestinian"))
            .AddPolicy(PolicyNames.AtLeast20,
             builder =>builder.AddRequirements(new MinimumAgeRequirement(20)))
            .AddPolicy(PolicyNames.CreatedAtLeast2Restaurants,
            builder => builder.AddRequirements(new CreateMultipleRestaurantRequirement(2)));
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        services.AddScoped<IAuthorizationHandler, CreateMultipleRestaurantRequirementHandler>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
    }
}
