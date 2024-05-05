using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Mapping.DTOs;

namespace qrwaiter_backend.Services.Interfaces
{
    public interface ITableService
    {
        Task<List<TableWithWaitersDTO>> GetTablesWithWaiterByRestaurantLink(string restaurantLink);
    }
}
