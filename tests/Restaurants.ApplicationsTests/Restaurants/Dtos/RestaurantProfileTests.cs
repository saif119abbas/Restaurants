using Xunit;
using AutoMapper;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Applications.Restaurants.Commands.CreateRestaurant;
using Restaurants.Applications.Restaurants.Commands.UpdateRestaurant;

namespace Restaurants.Applications.Restaurants.Dtos.Tests;

public class RestaurantProfileTests
{
    private readonly IMapper _mapper;
    public RestaurantProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantProfile>();
        });
        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        //arrange
        var restaurant = new Restaurant
        {
            Id=1,
            Name="Test",
            Description="This a test description",
            Address=new Address
            {

                City="Nablus",
                PostalCode="12-345",
                Street="123 Main Street"
            },
            Category="Indian",
            ContactEmail="test@test.com",
            ContactNumber="+970597335263",
            HasDelivery=true
        };
        //act 
        var restaurantDto= _mapper.Map<RestaurantDto>(restaurant);
        //assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(1);
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("This a test description");
        restaurantDto.Category.Should().Be("Indian");
        restaurantDto.City.Should().Be("Nablus");
        restaurantDto.PostalCode.Should().Be("12-345");
        restaurantDto.Street.Should().Be("123 Main Street");
        restaurantDto.HasDelivery.Should().Be(true);
    }
    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test",
            Description = "This a test description",
            City = "Nablus",
            PostalCode = "12-345",
            Street = "123 Main Street",
            Category = "Indian",
            ContactEmail = "test@test.com",
            ContactNumber = "+970597335263",
            HasDelivery = true
        };
        //act 
        var restaurant = _mapper.Map<Restaurant>(command);
        //assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.Address?.City.Should().Be(command.City);
        restaurant.Address?.PostalCode.Should().Be(command.PostalCode);
        restaurant.Address?.Street.Should().Be(command.Street);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }
    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new UpdateRestaurantCommand
        {
            Id=1,
            Name = "Test",
            Description = "This a test description",
            HasDelivery = true
        };
        //act 
        var restaurant = _mapper.Map<Restaurant>(command);
        //assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }
}