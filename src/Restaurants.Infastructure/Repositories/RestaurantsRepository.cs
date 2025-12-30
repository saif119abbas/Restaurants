
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infastructure.Repositories;
internal class RestaurantsRepository(RestaurantsDbContext context) : IRestaurantsRepository
{
    public async Task<int> Create(Restaurant restaurant)
    {
        var result= await context.Restaurants.AddAsync(restaurant);
        await context.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task Delete(Restaurant restaurant)
    {
        context.Restaurants.Remove(restaurant);
        await context.SaveChangesAsync();
    }

    public async  Task<IEnumerable<Restaurant>> GetAllAsync()
    { 
       var restaurants = await context.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return restaurants;
    }
    public async Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageNumber,
        int pageSize, 
        string? sortBy, 
        SortDirection sortDirection)
    {
        string query = searchPhrase?.ToLower();
        var basedQuery = context.Restaurants
             .Where(r => query == null || r.Name.ToLower().Contains(query) || r.Description.ToLower().Contains(query));
        var totalCount = await basedQuery.CountAsync();
        if(sortBy!=null)
        {
            var columnSelector=new Dictionary<string, Expression<Func<Restaurant, string>>>
            {
                {nameof(Restaurant.Name),r=>r.Name },
                {nameof(Restaurant.Category),r=>r.Category },
                {nameof(Restaurant.Description),r=>r.Description }
            };
            var selectedColumn=columnSelector[sortBy];
            basedQuery = sortDirection == SortDirection.Ascending
                ?basedQuery.OrderBy(selectedColumn):
                basedQuery.OrderByDescending(selectedColumn);
        }
        var restaurants=await basedQuery
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             
             .ToListAsync();
        return (restaurants,totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var retaurant = await context.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
        return retaurant;
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}

