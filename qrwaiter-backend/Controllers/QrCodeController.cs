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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace qrwaiter_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QrCodeController(
                                    //SignInManager<ApplicationUser> signInManager, 
                                    IHttpContextAccessor httpContextAccessor,
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_signInManager = signInManager;
            //_userManager = userManager;
        }
        [HttpGet("{id}")]
        public async Task<QrCodeDTO> Get([FromRoute] Guid id)
        {
            return _mapper.Map<QrCode, QrCodeDTO>(await _unitOfWork.QrCodeRepository.GetById(id));
        }
        // POST api/<RestaurantController>
        [HttpPost]
        public async Task<QrCodeDTO> Update([FromBody] QrCodeDTO qrCoodeDto)
        {
            var qrCode = await _unitOfWork.QrCodeRepository.GetById(qrCoodeDto.Id);
            _mapper.Map<QrCodeDTO, QrCode>(qrCoodeDto, qrCode);
            qrCode = await _unitOfWork.QrCodeRepository.Update(qrCode);
            _unitOfWork.SaveChanges();
            return _mapper.Map<QrCode, QrCodeDTO>(qrCode);
        }

        // PUT api/<RestaurantController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RestaurantController>/5
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
