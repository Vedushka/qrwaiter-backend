using qrwaiter_backend.Data.Models;

namespace qrwaiter_backend.Repositories.Interfaces
{
    public interface ITableRepository : IRepository<Table>
    {
        public Task<List<Table>> GetTablesByRestaurantId(Guid id);
    }
}
