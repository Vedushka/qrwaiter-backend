using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace qrwaiter_backend.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestaurantService _restaurantService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;


        public IdentityService(IConfiguration configuration,
                               IRestaurantService restaurantService,
                               IServiceProvider serviceProvider,
                               IHttpContextAccessor httpContextAccessor,
                               IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _restaurantService = restaurantService;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Refresh()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var jwsSettings = _configuration.GetSection("JwtSettings");
            var key = jwsSettings.GetValue<string>("Key");
            var issuer = jwsSettings.GetValue<string>("Issuer");
            var audience = jwsSettings.GetValue<string>("Audience");
            var expiration = jwsSettings.GetValue<int>("Expiration");
            var expirationRefreshDays = jwsSettings.GetValue<int>("ExpirationRefresDays");
            
            var expString = (_httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Exp) ?? throw new ArgumentNullException());
            var userId = Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? throw new ArgumentNullException());
            var exp = new DateTime(long.Parse(expString??""));
            if(exp >  exp.AddDays(expirationRefreshDays))
            {
                throw new UnauthorizedAccessException("Token has expired");
            }
            var user = _serviceProvider.GetService<SignInManager<ApplicationUser>>().UserManager.Users.FirstOrDefault(u => u.Id == userId);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("userId", user.Id.ToString()),
                new("restaurantId", user.IdRestaurant.ToString()),
            };
            var token = new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                claims: claims,
                                expires: DateTime.Now.AddSeconds(expiration),
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                            );
;
            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rawToken;
        }
        
            public async Task<string> Login(LoginRequest login)
        {
            var jwsSettings = _configuration.GetSection("JwtSettings");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = jwsSettings.GetValue<string>("Key");
            var issuer = jwsSettings.GetValue<string>("Issuer");
            var audience = jwsSettings.GetValue<string>("Audience");
            var expiration = jwsSettings.GetValue<int>("Expiration");

            var user = _serviceProvider.GetService<SignInManager<ApplicationUser>>().UserManager.Users.FirstOrDefault(u => u.NormalizedEmail == login.Email.ToUpper());

            var passCheck = await _serviceProvider.GetService<SignInManager<ApplicationUser>>().UserManager.CheckPasswordAsync(user, login.Password);
            if (!passCheck)
            {
                throw new UnauthorizedAccessException();
            }

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("userId", user.Id.ToString()),
                new("restaurantId", user.IdRestaurant.ToString()),
            };
            var token = new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                claims: claims,
                                expires: DateTime.Now.AddSeconds(expiration),
                                
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                            );

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rawToken;
        }

        public async Task Register(RegisterRequest registration)
        {
            await _unitOfWork.BeginTransactionAsync();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"Requires a user store with email support");
            }

            var userStore = _serviceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
            var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email))
            {
                throw new BadHttpRequestException("Invalid Email");
            }

            var user = new ApplicationUser();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, registration.Password);
            
            if (!result.Succeeded)
            {
                throw new BadHttpRequestException(result.Errors.First().Description);
            }

            await _restaurantService.CreateByRestaurantIdAndUserId(user.IdRestaurant, user.Id);
            _unitOfWork.SaveChanges();
            await _unitOfWork.CommitAsync();
            //await SendConfirmationEmailAsync(user, userManager, context, email);
        }
    }
}
