using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Services.Interfaces;
using System.Collections.Generic;
using System;

namespace qrwaiter_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public NotificationController(
                               IUnitOfWork unitOfWork,
                               INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        [AllowAnonymous]
        [HttpPost("addDeviceToQrCode")]
        public async Task<DeviceAndQrCodeDTO> AddDeviceToQrCode([FromBody] DeviceAndQrCodeDTO deviceAndQrCodeDto, [FromQuery] bool onlyThisDevice = false)
        {
            await _unitOfWork.BeginTransactionAsync();
            deviceAndQrCodeDto = await _notificationService.AddDeviceToQrCode(deviceAndQrCodeDto, onlyThisDevice);
            await _unitOfWork.CommitAsync();
            return deviceAndQrCodeDto;
        }
        [AllowAnonymous]
        [HttpGet("unsubscribeDeviceFromAllQrCodes/{deviceToken}")]
        public async Task<IActionResult> UnsubscribeDeviceFromAllQrCodes([FromRoute] string deviceToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            deviceToken = await _notificationService.UnsubscribeDeviceFromAllQrCodes(deviceToken);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("unsubscribeDeviceFromQrCode/{deviceToken}/{waiterLink}")]
        public async Task<IActionResult> UnsubscribeDeviceFromAllQrCodes([FromRoute] string deviceToken, string waiterLink)
        {
            await _notificationService.UnsubscribeDeviceFromQrCode(deviceToken, waiterLink);
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("callWaiter/{clientLink}")]
        public async Task<IActionResult> CallWaiter([FromRoute] string clientLink)
        {
            return Ok(await _notificationService.CallWaiter(clientLink));
        }
        
    }
}
