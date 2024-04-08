using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using qrwaiter_backend.Data.Models;
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
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RestaurantController(IRestaurantService restaurantService,
                                    //SignInManager<ApplicationUser> signInManager, 
                                    //UserManager<ApplicationUser> userManager
                                    IHttpContextAccessor httpContextAccessor,
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper
            )
        {
            _restaurantService = restaurantService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_signInManager = signInManager;
            //_userManager = userManager;
        }
        [HttpGet]
        public async Task<RestaurantDTO> Get()
        {
            var restaurantId = new Guid(_httpContextAccessor.HttpContext?.User.FindFirstValue("restaurantId") ?? throw new ArgumentNullException());
            return _mapper.Map<Restaurant, RestaurantDTO>(await _unitOfWork.RestaurantRepository.GetById(restaurantId));
        }
        [HttpGet("{id}")]
        public async Task<RestaurantDTO> Get([FromRoute] Guid id)
        {
            return _mapper.Map<Restaurant, RestaurantDTO>(await _unitOfWork.RestaurantRepository.GetById(id));
        }
        // POST api/<RestaurantController>
        [HttpPost]
        public async Task<RestaurantDTO> Update([FromBody] RestaurantDTO restaurantDto)
        {
            var restaurant = await _unitOfWork.RestaurantRepository.GetById(restaurantDto.Id);
            _mapper.Map<RestaurantDTO, Restaurant>(restaurantDto, restaurant);
            restaurant = await _unitOfWork.RestaurantRepository.Update(restaurant);
            _unitOfWork.SaveChanges();
            return _mapper.Map<Restaurant, RestaurantDTO>(restaurant);
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
    }
}
