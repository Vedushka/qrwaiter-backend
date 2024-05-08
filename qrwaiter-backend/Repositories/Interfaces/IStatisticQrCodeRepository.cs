using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;

namespace qrwaiter_backend.Repositories.Interfaces
{
    public interface IStatisticQrCodeRepository : IRepository<StatisticQrCode>
    {
        Task<StatisticQrCode?> GetLastByQrCodeId(Guid qrCodeId);
    }
}
