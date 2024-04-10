using qrwaiter_backend.Data.Models;

namespace qrwaiter_backend.Repositories.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        void SoftDelete(Guid id);
        Task<Restaurant> GenerateNewLink(Guid id);

    }
}
