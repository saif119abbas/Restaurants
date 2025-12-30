using AutoMapper;
using Restaurants.Applications.Dishes.Commands.CreateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Applications.Dishes.Dtos;
public class DishProfile:Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishCommand, Dish>();
    }
}

