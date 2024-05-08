using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;
using qrwaiter_backend.Mapping.DTOs;

namespace qrwaiter_backend.Services.Interfaces
{
    public interface IQrCodeService
    {
        Task<QrCodeAndTableDTO> GetQrCodeAndTableDTOByLink(string link, LinkType type, string deviceToken);
        Task<List<QrCodePrintDTO>> GetQrCodesForPrint(Guid restaurantId, LinkType linkType);
    }
}
