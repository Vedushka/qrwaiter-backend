using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;

namespace qrwaiter_backend.Repositories.Interfaces
{
    public interface IQrCodeRepository : IRepository<QrCode>
    {
        void SoftDelete(Guid id);
        Task<QrCode> GenerateNewLink(Guid id, LinkType type);
    }
}
