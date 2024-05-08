using AutoMapper;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace qrwaiter_backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public NotificationService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment
                                   )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<DeviceAndQrCodeDTO> AddDeviceToQrCode(DeviceAndQrCodeDTO deviceAndQrCodeDto, bool onlyThisDevice)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetByDeviceToken(deviceAndQrCodeDto.DeviceToken);
            QrCode? qrCode = await _unitOfWork.QrCodeRepository.GetByLink(deviceAndQrCodeDto.Link, LinkType.WaiterLink);
            if (qrCode is null) throw new NullReferenceException(nameof(qrCode));
            if (onlyThisDevice)
            {
                qrCode.NotifyDevices.Clear();
                _unitOfWork.SaveChanges();
            }

            if (device is null)
            {
                device = new Device
                {
                    Name = deviceAndQrCodeDto.Name,
                    DeviceToken = deviceAndQrCodeDto.DeviceToken,
                };
                device.QrCodes.Add(qrCode);
                device = await _unitOfWork.DeviceRepository.Insert(device);
                _unitOfWork.SaveChanges();
                return deviceAndQrCodeDto;
            }
            else
            {
                deviceAndQrCodeDto.Name = device.Name;
                if (qrCode.NotifyDevices.Any(d => d.DeviceToken == deviceAndQrCodeDto.DeviceToken))
                {
                    return deviceAndQrCodeDto;
                }
                else
                {
                    qrCode.NotifyDevices.Add(device);
                    _unitOfWork.SaveChanges();
                    return deviceAndQrCodeDto;
                }

            }

        }

        public async Task<string> CallWaiter(string clientLink)
        {
            try
            {

            QrCode? qrCode = await _unitOfWork.QrCodeRepository.GetByLink(clientLink, LinkType.ClientLink);
            if (qrCode is null) throw new NullReferenceException(nameof(qrCode));

            await _unitOfWork.StatisticQrCodeRepository.Insert(new StatisticQrCode{ IdQrCode = qrCode.Id});
            _unitOfWork.SaveChanges();               
            
            string title = $"{qrCode.Table.Name} {qrCode.Table.Number}";
            string body = "Вызов официанта";
            List<string> tokens = qrCode.NotifyDevices.Select(d => d.DeviceToken).ToList(); 
            var responses = await this.SendWebPushMulticast(tokens, title, body);
            return "ok";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> UnsubscribeDeviceFromAllQrCodes(string deviceToken)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetByDeviceToken(deviceToken);
            if (device is null) throw new NullReferenceException(nameof(device));
            device.QrCodes.Clear();
            await _unitOfWork.DeviceRepository.Update(device);
            _unitOfWork.SaveChanges();
            return deviceToken;
        }

        public async Task<string> UnsubscribeDeviceFromQrCode(string deviceToken, string waiterLink)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetByDeviceToken(deviceToken);
            QrCode? qrCode = await _unitOfWork.QrCodeRepository.GetByLink(waiterLink, LinkType.WaiterLink);
            if (qrCode is null) throw new NullReferenceException(nameof(qrCode));
            if (device is null) throw new NullReferenceException(nameof(device));

            device.QrCodes.Remove(qrCode);
            await _unitOfWork.DeviceRepository.Update(device);
            _unitOfWork.SaveChanges();
            return deviceToken;
        }
        public async Task<BatchResponse?> SendWebPushMulticast(List<string> deviceTokens, string title, string body)
        {
            var path = _hostingEnvironment.ContentRootPath + "/serviceAccountKey.json";
            var credentialJson = await System.IO.File.ReadAllTextAsync(path);
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(credentialJson)
                });
            }
            var message = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Tokens = deviceTokens

            };
            return await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

        }

    }
}
