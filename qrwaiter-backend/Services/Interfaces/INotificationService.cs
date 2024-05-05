using FirebaseAdmin.Messaging;
using qrwaiter_backend.Mapping.DTOs;

namespace qrwaiter_backend.Services.Interfaces
{
    public interface INotificationService
    {
        Task<DeviceAndQrCodeDTO> AddDeviceToQrCode(DeviceAndQrCodeDTO deviceAndQrCodeDto, bool onlyThisDevice);
        Task<string> UnsubscribeDeviceFromAllQrCodes(string deviceToken);
        Task<string> UnsubscribeDeviceFromQrCode(string deviceToken, string waiterLink);
        Task<string> CallWaiter(string clientLink);
        Task<BatchResponse?> SendWebPushMulticast(List<string> deviceTokens, string title, string body);

    }
}
