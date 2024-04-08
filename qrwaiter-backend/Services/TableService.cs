using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services.Interfaces;

namespace qrwaiter_backend.Services
{
    public class TableService : ITableService 
    {
        private readonly ITableRepository _tableRepository;
        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        //public async Task<Table> CreateByTableAndQrCodeDTO()
        //{
        //        var restaurant = new () { Id = resturantId, IdUser = userId };
        //        return await _restaurantRepository.Insert(restaurant);
        //}
    }
}
