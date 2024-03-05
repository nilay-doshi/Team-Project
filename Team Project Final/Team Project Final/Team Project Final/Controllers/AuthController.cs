using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Team.Repo.Models;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team_Project_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
         }

        [HttpPost("adduser")]
        public async Task<IActionResult> adduser(UserRegistration userRegistration)
        {
            return Ok(await _authService.Adduser(userRegistration));
        }

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken(UserLoginDTO userlogindto)
        {
            return Ok(await _authService.GetTokenAsync(userlogindto));
        }

        [Authorize]
        [HttpPost("updatepassword")]
        public async Task<IActionResult> updatepassword(UpdatepasswordDTO updatePassworddto)
        {
            try
            {
                var updatePassword = await _authService.updatepassword(updatePassworddto);
                return Ok(updatePassword);
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }
        }

    }
}

