using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;

namespace qrwaiter_backend.Repositories.Interfaces
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<Device?> GetByDeviceToken(string token);
    }
}
