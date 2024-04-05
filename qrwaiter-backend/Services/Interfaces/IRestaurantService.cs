using qrwaiter_backend.Data.Models;

namespace qrwaiter_backend.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<Restaurant> CreateByRestaurantIdAndUserId(Guid resturantId, Guid userId);
    }
}
