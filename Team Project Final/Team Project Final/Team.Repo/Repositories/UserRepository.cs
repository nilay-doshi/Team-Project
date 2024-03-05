using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team.Repo.Interface;
using Team.Repo.Models;

namespace Team.Repo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamDBContext _dbContext;
        public UserRepository(TeamDBContext teamDBContext)
        {
            _dbContext = teamDBContext;
        }
        public async Task Adduser(UserRegistration userRegistration)
        {
            try
            {
                 await _dbContext.Registration.AddAsync(userRegistration);
                 await _dbContext.SaveChangesAsync();
               
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<UserRegistration> CheckUserAuthAsync(string email, string password)
        {
            try
            {
                var user = await _dbContext.Registration.FirstOrDefaultAsync(u => u.Email == email);

                return user;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<string> GetUserByEmail1(string email)
        {
            try
            {
                var user = await _dbContext.Registration.FirstOrDefaultAsync(u => u.Email == email);
                user.Password = null;
                return user.FlagRole.ToString();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<string> updatePassword(string email, string password)
        {
            try
            {
                var user = await _dbContext.Registration.
                            Where(u => u.Email == email)
                            .FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Password = password;
                    _dbContext.SaveChangesAsync();
                    user.Password = null;
                    password = null;
                    return "Password updated successfully";
                }
                else
                {
                    return "User not found";
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
    }
}
