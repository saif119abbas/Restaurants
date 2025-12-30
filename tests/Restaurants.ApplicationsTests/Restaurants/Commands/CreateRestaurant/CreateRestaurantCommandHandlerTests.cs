using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Applications.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Applications.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnCreateRestaurantId()
    {
        //assert
        var loggerMock=new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock=new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", [],null,null);
        userContextMock.Setup(u=>u.GetCurrentUser()).Returns(currentUser);

        var commnadHanlder = new CreateRestaurantCommandHandler
            (
                restaurantRepositoryMock.Object,
                mapperMock.Object,
                loggerMock.Object,
                userContextMock.Object
            );
        //act
        var result=await commnadHanlder.Handle( command,CancellationToken.None );
        //assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be(currentUser.Id); 
        restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);

    }
}