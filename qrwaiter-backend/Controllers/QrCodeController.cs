using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Services;
using qrwaiter_backend.Services.Interfaces;
using System.Linq;
using System.Security.Claims;


namespace qrwaiter_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQrCodeService _qrCodeService;
        public QrCodeController(
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper,
                                    IQrCodeService qrCodeService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _qrCodeService = qrCodeService;
        }
        [HttpGet("{id}")]
        public async Task<QrCodeDTO> Get([FromRoute] Guid id)
        {
            return _mapper.Map<QrCode, QrCodeDTO>(await _unitOfWork.QrCodeRepository.GetById(id));
        }
        [AllowAnonymous]
        [HttpGet("QrCodeAndTableDto/{link}/{linkType}")]
        public async Task<QrCodeAndTableDTO> GetQrCodeAndTableDTOByLink([FromRoute] string link, LinkType linkType = LinkType.WaiterLink, [FromQuery] string? deviceToken = "")
        {
            return await _qrCodeService.GetQrCodeAndTableDTOByLink(link, linkType, deviceToken);
        }
        [HttpPost]
        public async Task<QrCodeDTO> Update([FromBody] QrCodeDTO qrCoodeDto)
        {
            var qrCode = await _unitOfWork.QrCodeRepository.GetById(qrCoodeDto.Id);
            _mapper.Map<QrCodeDTO, QrCode>(qrCoodeDto, qrCode);
            qrCode = await _unitOfWork.QrCodeRepository.Update(qrCode);
            _unitOfWork.SaveChanges();
            return _mapper.Map<QrCode, QrCodeDTO>(qrCode);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("generateLink/{id}/{type}")]
        public async Task<QrCodeDTO> GenerateLink([FromRoute] Guid id, LinkType type)
        {
            var qrCode = await _unitOfWork.QrCodeRepository.GenerateNewLink(id, type);
            _unitOfWork.SaveChanges();
            return _mapper.Map<QrCode, QrCodeDTO>(qrCode);
        }
    }
}
