
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infastructure.Persistence;

namespace Restaurants.Infastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext context) : IDishesRepository
{
    public async Task<int> Create(Dish dish)
    {
        await context.Dishes.AddAsync(dish);
        await context.SaveChangesAsync();
        return dish.Id;
    }

    public async Task Delete(IEnumerable<Dish> dishes)
    {
         context.Dishes.RemoveRange(dishes);
         await SaveChanges();
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}
