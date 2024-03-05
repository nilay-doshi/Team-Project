using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team.Repo.Models;

namespace Team.Repo.Interface
{
    public interface IUserRepository
    {
        Task Adduser(UserRegistration userRegistration);
        Task<UserRegistration> CheckUserAuthAsync(string email, string password);
        Task<string> GetUserByEmail1(string email);
        Task<string> updatePassword(string email, string Password);
    }
}
