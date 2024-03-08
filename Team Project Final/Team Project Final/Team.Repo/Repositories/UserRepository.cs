using Microsoft.EntityFrameworkCore;
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

        #region Add User
        public async Task<UserRegistration> Adduser(UserRegistration userRegistration)
        {
            try
            {

                var user1 = await checkEmailexists(userRegistration.Email);
                if (user1)
                {
                    return null;
                }

                await _dbContext.Registration.AddAsync(userRegistration);
                await _dbContext.SaveChangesAsync();

                var user = await CheckUserAuthAsync(userRegistration.Email, userRegistration.Password);
                user.Password = null;
                return user;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region Check Email Exists
        public async Task<bool> checkEmailexists(string email)
        {
            try
            {
                return await _dbContext.Registration.AnyAsync(u => u.Email == email);

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region Check User Authentication
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
        #endregion

        #region Get User by email

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
        #endregion

        #region Update password 
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
                    await _dbContext.SaveChangesAsync();
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
        #endregion
    }
}
