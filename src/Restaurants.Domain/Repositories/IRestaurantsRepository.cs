
using Restaurants.Domain.Constatnts;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;
public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase,int pageNumber,int pageSize,string?sortBy,SortDirection sortDirection);
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> Create(Restaurant restaurant);
    Task Delete(Restaurant restaurant);
    Task SaveChanges();
}

