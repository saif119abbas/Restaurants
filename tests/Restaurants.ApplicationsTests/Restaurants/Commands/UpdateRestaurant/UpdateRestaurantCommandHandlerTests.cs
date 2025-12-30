using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using Xunit;

namespace Restaurants.Applications.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantServiceAuthorizationMock;
    private readonly UpdateRestaurantCommandHandler _handler;
    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _restaurantServiceAuthorizationMock = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler
            (
                _mapperMock.Object,
                _restaurantRepositoryMock.Object,
                _loggerMock.Object,
                _restaurantServiceAuthorizationMock.Object
            );
    }


    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        //arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id=restaurantId,
            Name="Test",
            Description="This a short description",
            HasDelivery=true
        };
        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test2",
            Description = "This a long description",
            HasDelivery = true
        };

        _mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _restaurantServiceAuthorizationMock.Setup(s => s.Authorize(restaurant, ResourceOperation.Update)).Returns(true);
        //action
        await _handler.Handle(command, CancellationToken.None);
        //assert
        _restaurantRepositoryMock.Verify(r=>r.SaveChanges(),Times.Once);
        _mapperMock.Verify(m=>m.Map(command,restaurant), Times.Once);
    }
    [Fact()]
    public async Task Handle_WithNonExisitingRestaurant_ShouldThrowNotFoundExceptionException()
    {
        //arrange
        var restaurantId = 2;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "This a short description",
            HasDelivery = true
        };

        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant)null!);
        //action
        Func<Task>action=async()=>await _handler.Handle(command, CancellationToken.None);
        //assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(Restaurant)} with id:{restaurantId} is not found");
    }
    [Fact()]
    public async Task Handle_WithUnAuthorizedUser_ShouldThrowForbidException()
    {
        //arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "This a short description",
            HasDelivery = true
        };
        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test2",
            Description = "This a long description",
            HasDelivery = true
        };

        _mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _restaurantServiceAuthorizationMock.Setup(s => s.Authorize(restaurant, ResourceOperation.Update)).Returns(false);
        //action
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);
        //assert
        await action.Should().ThrowAsync<ForbidException>();
    }
}