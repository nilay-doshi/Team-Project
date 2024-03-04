using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Team_Project.EmailService;
using Team_Project.Models;
using Team_Project.Repository;

namespace Team_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<UserRegistration> _passwordHasher;
        private readonly IPasswordHasher<ForgotPasswordDTO> _passwordHasher1;
        private readonly TeamDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(IUserRepository userRepository, IEmailService emailService, IPasswordHasher<UserRegistration> passwordHasher, IPasswordHasher<ForgotPasswordDTO> passwordHasher1, IConfiguration configuration, TeamDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {

            _userRepository = userRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _passwordHasher1 = passwordHasher1;
            _configuration = configuration;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> addUser([FromBody] UserRegistration userRegistration)
        {
            try
            {
                if (userRegistration.Email == null || userRegistration.Password == null)
                {
                    return BadRequest("Invalid User Data");
                }

                userRegistration.Password = "team1234";
                userRegistration.Password = _passwordHasher.HashPassword(userRegistration, userRegistration.Password);
                var user = await _userRepository.AddUser(userRegistration);
                await _emailService.SendEmail(userRegistration.Email, "Welcome to our platform " + userRegistration.FirstName, "Thank you for signing up! " + userRegistration.FirstName + " " + userRegistration.LastName);

                //  userLogin.Password.GetHashCode();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost("loginwithoutJwt")]
        public async Task<IActionResult> loginwithoutJwt(UserloginDTO userlogin)
        {
            if (userlogin == null)
            {
                return BadRequest("invalid login details");
            }
            try
            {
                var user = await _userRepository.GetUserByEmail(userlogin.Email, userlogin.Password);

                if (user == null)
                    return NotFound("user not found");

                if (user.Equals("Success"))
                {
                    return Ok("login successful");
                }
                else
                {
                    return Unauthorized("Invalid Credentials");
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, "internal server error");
            }

        }

        [AllowAnonymous]
        [HttpPost("loginwithJwt")]
        public async Task<ActionResult<string>> loginwithJwt(UserloginDTO userlogin)
        {
            if (userlogin == null || string.IsNullOrEmpty(userlogin.Email) || string.IsNullOrEmpty(userlogin.Password))
                return BadRequest("Invalid Login details");
            var user = await _userRepository.GetUserByEmail(userlogin.Email, userlogin.Password);

            string token = CreateToken(userlogin);

            //  var resultpassword = _passwordHasher.VerifyHashedPassword(user, user.Password, userlogin.Password);

            if (user == null)
                return NotFound("user not found");

            return Ok(token);
        }
        private string CreateToken(UserloginDTO userlogin)
        {
            var user = _userRepository.GetUserByEmail1(userlogin.Email);

            string FlagRole = user.Value;
            Console.WriteLine(FlagRole);

            List<Claim> claims = new List<Claim>
            {
            new Claim("email", userlogin.Email),
            new Claim(ClaimTypes.Role, FlagRole)
            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddDays(1),
                 signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [Authorize]
        [HttpPost("updatePassword")]
        public string updatePassoword(ForgotPasswordDTO forgotpassword)
        {
            try
            {
                if (forgotpassword == null || string.IsNullOrEmpty(forgotpassword.newPassword))
                    return BadRequest("Enter Valid Password").ToString();

                string emailClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value ?? "";
                
             //   if (string.IsNullOrEmpty(emailAddress))
              //      return BadRequest("Enter Valid Password to your login").ToString();

                forgotpassword.newPassword = _passwordHasher1.HashPassword(forgotpassword, forgotpassword.newPassword);
                var updatePassword = _userRepository.updatePassword(emailClaim, forgotpassword.newPassword);
                _emailService.SendEmail(emailClaim, "Password changed", "Your password has been updated. Thank You");
               
                return updatePassword.ToString();
            }
            catch(Exception ex) 
            {
                string errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }
        }

    }
}
