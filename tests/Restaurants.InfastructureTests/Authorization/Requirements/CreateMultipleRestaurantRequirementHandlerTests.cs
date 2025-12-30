using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Applications.Users;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infastructure.Authorization.Requirements.Tests;

public class CreateMultipleRestaurantRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsyncTest_UserHasCreatedMutlipleRestaurants_ShouldSucceed()
    {
        //arrange
        var loggerMock=new Mock<ILogger<CreateMultipleRestaurantRequirementHandler>>();
        var userContextMock = new Mock<IUserContext>();
        var restaurantsRepositoryMock=new Mock<IRestaurantsRepository>();
        var currentUser = new CurrentUser("user-id", "test@gmail.com", [UserRoles.Owner], null, null);
        userContextMock.Setup(u=>u.GetCurrentUser()).Returns(currentUser);
        var restaurants = new List<Restaurant>
        {
            new()
            {
                Id = 1,
                Name = "Test1",
                OwnerId = currentUser.Id,
                Description = "test",
                Category = "Indian"

            },
             new()
             {
                 Id = 2,
                 Name = "Test2",
                 OwnerId = currentUser.Id,
                 Description = "test",
                 Category = "Indian"

             },
            new()
            {
                Id = 3,
                Name = "Test3",
                OwnerId = "2",
                Description = "test",
                Category = "Indian"

            }
        };

        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        var requirement = new CreateMultipleRestaurantRequirement(2);
        var context = new AuthorizationHandlerContext([requirement],null,null);
        var requirementHandler = new CreateMultipleRestaurantRequirementHandler
            (
                loggerMock.Object, 
                userContextMock.Object, 
                restaurantsRepositoryMock.Object
            );

        //act
        await requirementHandler.HandleAsync(context);
        //assert
        context.HasSucceeded.Should().BeTrue();

    }
    [Fact()]
    public async Task HandleRequirementAsyncTest_UserHasNotCreatedMutlipleRestaurants_ShouldFail()
    {
        //arrange
        var loggerMock = new Mock<ILogger<CreateMultipleRestaurantRequirementHandler>>();
        var userContextMock = new Mock<IUserContext>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        var currentUser = new CurrentUser("user-id", "test@gmail.com", [UserRoles.Owner], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);
        var restaurants = new List<Restaurant>
        {
            new()
            {
                Id = 1,
                Name = "Test1",
                OwnerId = currentUser.Id,
                Description = "test",
                Category = "Indian"

            },
             new()
             {
                 Id = 2,
                 Name = "Test2",
                 OwnerId = "2",
                 Description = "test",
                 Category = "Indian"

             },
            new()
            {
                Id = 3,
                Name = "Test3",
                OwnerId = "2",
                Description = "test",
                Category = "Indian"

            }
        };

        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        var requirement = new CreateMultipleRestaurantRequirement(2);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        var requirementHandler = new CreateMultipleRestaurantRequirementHandler
            (
                loggerMock.Object,
                userContextMock.Object,
                restaurantsRepositoryMock.Object
            );

        //act
        await requirementHandler.HandleAsync(context);
        //assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();

    }
}