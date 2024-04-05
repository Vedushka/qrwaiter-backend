using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace qrwaiter_backend.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<string> Login(LoginRequest login);
        Task<string> Refresh();
        Task Register(RegisterRequest registration);
    }
}
