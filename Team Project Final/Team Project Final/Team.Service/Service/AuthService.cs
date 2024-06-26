﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Team.Repo.Enums;
using Team.Repo.Interface;
using Team.Repo.Models;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team.Service.Service
{
    public class AuthService : IAuthService
    {
        
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<UserRegistration> _passwordHasher;
        private readonly IPasswordHasher<UpdatepasswordDTO> _passwordHasher1;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        static int enumcountId = (int)CountEnum.FlagCount;
        static int enumUserId = (int)RoleEnum.User;


        #region Constructor

        public AuthService(IEmailService emailService, IPasswordHasher<UserRegistration> passwordHasher, IPasswordHasher<UpdatepasswordDTO> passwordHasher1, IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher1 = passwordHasher1;
        }

        #endregion

        #region Register User

        public async Task<ResponseDTO> Adduser(UserRegistration userRegistration)
        {
            try
            {
                
                userRegistration.Password = "team1234";
                userRegistration.FlagRole = enumUserId;
                userRegistration.FlagCouunt = enumcountId;
                userRegistration.Password = _passwordHasher.HashPassword(userRegistration, userRegistration.Password);
                var user = await _userRepository.Adduser(userRegistration);

                if (user == null)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Email already exists"
                    };
                }

                await _emailService.SendEmail(userRegistration.Email, "Welcome to our platform " + userRegistration.FirstName, "Thank you for signing up! on our portal " + userRegistration.FirstName + " " + userRegistration.LastName + " . You can login using your email : " + userRegistration.Email);
                userRegistration.Password = null;
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Data Entered Success",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Error = ex.Message };
            }
        }

        #endregion

        #region gettoken
        public async Task<ResponseDTO> GetTokenAsync(UserLoginDTO userlogin)
        {
            try
            {
                if (userlogin == null)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Email or Password is null"
                    };
                }
                var user = await _userRepository.CheckUserAuthAsync(userlogin.Email, userlogin.Password);

                var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, userlogin.Password);

                if (resultPassword == PasswordVerificationResult.Success)
                {
                    userlogin.Password = null;
                    user.Password = null;

                    var token = CreateToken(userlogin);

                    if (user == null)
                        return new ResponseDTO { Message = "User not found " };

                    return new ResponseDTO { Status = 200, Message = "Success", token = token };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Error = ex.Message };
            }
        }
        #endregion

        #region Update Password
        public async Task<ResponseDTO> updatepassword(UpdatepasswordDTO updatepassworddto)
        {
            if (updatepassworddto == null || string.IsNullOrEmpty(updatepassworddto.newPassword))
            {
                return new ResponseDTO { Message = "Enter valid password" };
            }
            var emailClam = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
            string emailClaim = null;
            if (emailClam != null)
            {
                emailClaim = emailClam.Value;

                updatepassworddto.newPassword = _passwordHasher1.HashPassword(updatepassworddto, updatepassworddto.newPassword);
                var updatePassword = await _userRepository.updatePassword(emailClaim, updatepassworddto.newPassword);
                await _emailService.SendEmail(emailClaim, "Password changed", "Your password has been updated. You can login using your new password on our portal ");
                return new ResponseDTO { Status = 200, Message = "Success", allData = updatePassword.ToString() };
            }
            return new ResponseDTO { Status = 400, Message = "Error in code" };

        }
        #endregion

        #region Creating token

        private string CreateToken(UserLoginDTO userlogin)
        {
            var user = _userRepository.GetUserByEmail1(userlogin.Email);

            string FlagRole = user.Result;
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
        #endregion
    }
}
