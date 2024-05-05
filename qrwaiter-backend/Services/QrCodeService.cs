using Microsoft.IdentityModel.Tokens;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services.Interfaces;

namespace qrwaiter_backend.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        public QrCodeService(IQrCodeRepository qrCodeRepository)
        {
            _qrCodeRepository = qrCodeRepository;
        }

        public async Task<QrCodeAndTableDTO> GetQrCodeAndTableDTOByLink(string link, LinkType type, string deviceToken)
        {
            var qrCode = await _qrCodeRepository.GetByLink(link, type);
            QrCodeAndTableDTO qrCodeAndTableDTO = new QrCodeAndTableDTO
            {
                ClientLink = qrCode.ClientLink,
                WaiterLink = qrCode.WaiterLink,
                QrTitle = qrCode.Title,
                TableName = qrCode.Table.Name,
                TableNumber = qrCode.Table.Number
            };
            if (!deviceToken.IsNullOrEmpty())
            {
                qrCodeAndTableDTO.subscribed = qrCode.NotifyDevices.Any(d => d.DeviceToken == deviceToken);
            }
            return qrCodeAndTableDTO;
        }
    }
}
