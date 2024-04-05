using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using qrwaiter_backend.Data.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Azure.Security.KeyVault.Certificates;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using qrwaiter_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace qrwaiter_backend.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService  identityService)
        {
            _identityService = identityService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            return  Ok(new { token = await _identityService.Login(login) });
        }
        //to do
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            return Ok(new { token = await _identityService.Refresh() });
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registration)
        {
            await _identityService.Register(registration);
            return Ok();
        }
    }
}
