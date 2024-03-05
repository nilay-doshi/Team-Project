using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team.Repo.Models;
using Team.Service.DTO;

namespace Team.Service.Interface
{
    public interface IAuthService
    {
        public Task<ResponseDTO> Adduser(UserRegistration userRegistration);
        public  Task<ResponseDTO> GetTokenAsync(UserLoginDTO userlogindto);
        public Task<ResponseDTO> updatepassword(UpdatepasswordDTO updatepassworddto);
    }
}
