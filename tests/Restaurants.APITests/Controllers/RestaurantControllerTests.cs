using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.APITests;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Applications.Restaurants.Dtos;
using Restaurants.Infastructure.Seedrs;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantControllerTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository > _restaurantsRepositoryMock=new();

    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder=>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IPolicyEvaluator,FakePolicyEvalutor>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _=> _restaurantsRepositoryMock.Object));
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Retusn200Ok()
    {
        //arrange
        var clinet=_factory.CreateClient();
        //act
        var result = await clinet.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

    }
    [Fact()]
    public async Task GetAll_ForInValidRequest_Return400BadRequest()
    {
        //arrange
        var clinet = _factory.CreateClient();
        //act
        var result = await clinet.GetAsync("/api/restaurants");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }
    [Fact()]
    public async Task GetById_ForNonExisting_ShouldReturn404NotFound()
    {
        //arrange

        var clinet = _factory.CreateClient();
        var id = 1111111;
        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        //act
        var result = await clinet.GetAsync($"/api/restaurants/{id}");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        //arrange

        var clinet = _factory.CreateClient();
        var id = 99;
        var restaurant = new Restaurant()
        {
            Id = id,
            Name="Test",
            Category="Indian",
            Description="Simple test"
        };
        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(restaurant);
        //act
        var result = await clinet.GetAsync($"/api/restaurants/{id}");
        var restaurnatDto =await  result.Content.ReadFromJsonAsync<RestaurantDto>();
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurnatDto.Should().NotBeNull();
        restaurnatDto.Id.Should().Be(restaurant.Id);
        restaurnatDto.Name.Should().Be(restaurant.Name);
        restaurnatDto.Description.Should().Be(restaurant.Description);
        restaurnatDto.Category.Should().Be(restaurant.Category);


    }

}