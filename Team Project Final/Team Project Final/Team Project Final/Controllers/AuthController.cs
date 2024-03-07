using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Team.Repo.Models;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team_Project_Final.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

#region constructor
        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
         }
        #endregion

#region User Registration
        [HttpPost("adduser")]
        public async Task<IActionResult> adduser(UserRegistration userRegistration)
        {
            return Ok(await _authService.Adduser(userRegistration));
        }
        #endregion

#region Get Token

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetToken(UserLoginDTO userlogindto)
        {
            return Ok(await _authService.GetTokenAsync(userlogindto));
        }
        #endregion

#region Update Password
        [Authorize]
        [HttpPut("updatepassword")]
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
#endregion

    }
}

