using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services.Interfaces;

namespace qrwaiter_backend.Services
{
    public class RestaurantService : IRestaurantService 
    {
        private readonly IRestaurantRepository _restaurantRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Restaurant> CreateByRestaurantIdAndUserId(Guid resturantId, Guid userId)
        {
                var restaurant = new Restaurant() { Id = resturantId, IdUser = userId };
                return await _restaurantRepository.Insert(restaurant);
        }
    }
}
