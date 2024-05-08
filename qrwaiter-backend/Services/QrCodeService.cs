using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IConfiguration _configuration;

        public QrCodeService(IQrCodeRepository qrCodeRepository,
                             IRestaurantRepository restaurantRepository,
                             IConfiguration configuration)
        {
            _qrCodeRepository = qrCodeRepository;
            _restaurantRepository = restaurantRepository;
            _configuration = configuration;
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

        public async Task<List<QrCodePrintDTO>> GetQrCodesForPrint(Guid restaurantId, LinkType linkType)
        {
            var restaurant = await _restaurantRepository.GetWithTablesAndQrCodes(restaurantId);
            var qrCodes = restaurant.Tables.Select(t => t.QrCode).OrderBy(qr => qr.Table.Number).ToList();
            string clientUrl = _configuration.GetValue<string>("ClientUrl");
            List<QrCodePrintDTO> qrCodesPrintDto = new List<QrCodePrintDTO>();
            if (linkType.Equals(LinkType.WaiterLink))
            {
                clientUrl += "/qr/subscribe/"; 
                qrCodes.ForEach(qrCode =>
                {
                    qrCodesPrintDto.Add(new QrCodePrintDTO { Link = clientUrl + qrCode.WaiterLink, Title = $"*{qrCode.Table.Name} {qrCode.Table.Number}" });
                });
            }
            else
            {
                clientUrl += "/qr/call/"; 
                qrCodes.ForEach(qrCode =>
                {
                    qrCodesPrintDto.Add(new QrCodePrintDTO { Link = clientUrl + qrCode.ClientLink, Title =  $"{qrCode.Title}\n{qrCode.Table.Name} {qrCode.Table.Number}" });
                });
            }
            return qrCodesPrintDto;
        }
    }
}
