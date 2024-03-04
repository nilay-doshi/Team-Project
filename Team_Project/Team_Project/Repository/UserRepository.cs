using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team_Project.Models;

namespace Team_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamDbContext _dbContext;
        private readonly IPasswordHasher<UserRegistration> _passwordHasher;


        public UserRepository(TeamDbContext dbContext, IPasswordHasher<UserRegistration> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserRegistration> AddUser(UserRegistration userRegistration)
        {
            try
            {
                if (userRegistration == null)
                {
                    throw new ArgumentNullException(nameof(userRegistration));
                }
                var checkEmailexists = _dbContext.Registration.FirstOrDefault(u => u.Email == userRegistration.Email);
                if (checkEmailexists != null)
                    return checkEmailexists;

                await _dbContext.Registration.AddAsync(userRegistration);
                await _dbContext.SaveChangesAsync();

                userRegistration.Password = null;

            return userRegistration;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<ActionResult<string>> GetUserByEmail(string email, string password)
        {
            try
            {
                var user = await _dbContext.Registration.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                    return "null";

                var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (resultPassword == PasswordVerificationResult.Success)
                {
                    password = null;
                    user.Password = null;
                    return "Success";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public ActionResult<string> GetUserByEmail1(string email)
        {
            try
            {
                var user = _dbContext.Registration.FirstOrDefault(u => u.Email == email);
                user.Password = null;
                return user.FlagRole.ToString();
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public string updatePassword(string email, string password)
        {
            try
            {
                var user = _dbContext.Registration.
                            Where(u=> u.Email == email)
                            .FirstOrDefault();
                if(user != null)
                {
                    user.Password = password;
                    _dbContext.SaveChanges();
                    user.Password = null;

                    return "Password updated successfully";
                }
                else
                {
                    return "User not found";
                }

            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);    
            }
        }

        public async Task<List<UserRegistration>> SavePlayers(string[] playersEmail)
        {
            try
            {
                var Checkplayerscount = await _dbContext.Registration
                                        .CountAsync(u => u.FlagRole == 1);

                var Checkcaptaincount = await _dbContext.Registration.CountAsync(u => u.FlagRole == 2);
                var afteraddCount = 0;
                if (Checkcaptaincount > 0)
                {
                    afteraddCount = Checkplayerscount + playersEmail.Length + 1;
                }

                if (Checkcaptaincount == 0)
                {
                    afteraddCount = Checkplayerscount + playersEmail.Length;
                }

                if (Checkplayerscount > 14 || afteraddCount > 15)
                {
                    return null;
                }

                var users = _dbContext.Registration
                            .Where(u => playersEmail.Contains(u.Email) && u.FlagRole == 0)
                            .ToList();
                
                foreach (var user in users)
                {
                    user.FlagRole = 1;
                }
                _dbContext.SaveChanges();
                foreach (var user in users)
                {
                    user.Password = null;
                }
                return users;

            }
            catch(Exception ex) 
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public UserRegistration? SaveCaptain(string captainEmail)
        {
            try
            {
                var checkCaptain = _dbContext.Registration
           .FirstOrDefault(u => u.FlagRole == 2);
                if (checkCaptain != null)
                {
                    return checkCaptain;
                }

                var makeCaptain = _dbContext.Registration
                                   .FirstOrDefault(u => u.Email == captainEmail && u.FlagRole == 1);
                if (makeCaptain != null)
                {
                    makeCaptain.FlagRole = 2;
                    _dbContext.SaveChanges();
                }
                makeCaptain.Password = null;
                return makeCaptain;
            }
            catch(Exception ex )
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<List<UserRegistration?>> SavePlayersByCaptain(string[] playersEmail)
        {
            try
            {
                var Checkplayerscount = await _dbContext.Registration
                                        .CountAsync(u => u.FlagRole == 1);

                var Checkcaptaincount = await _dbContext.Registration.CountAsync(u => u.FlagRole == 2);

                var afteraddCount = 0;
                if (Checkcaptaincount > 0)
                {
                    afteraddCount = Checkplayerscount + playersEmail.Length + 1;
                }

                if (Checkcaptaincount == 0)
                {
                    afteraddCount = Checkplayerscount + playersEmail.Length;
                }

                afteraddCount = Checkplayerscount + playersEmail.Length;

                if (Checkplayerscount + Checkcaptaincount > 15 || afteraddCount > 15 || playersEmail.Length > 10)
                {
                    return null;
                }

                var users = _dbContext.Registration
                           .Where(u => playersEmail.Contains(u.Email) && u.FlagRole == 0)
                           .ToList();

                foreach (var user in users)
                {
                    user.FlagRole = 1;
                }

                _dbContext.SaveChanges();
                foreach (var user in users)
                {
                    user.Password = null;
                }

                return users;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        public UserRegistration? verifyCaptain(string captainEmail)
        {
            try
            {
                var verify = _dbContext.Registration.FindAsync(captainEmail);
                verify.Result.Password = null;
                
                return verify.Result;

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }

        }
        public int getPlayerCount()
        {
            try
            {
                var countPlayer = _dbContext.Registration.CountAsync(u => u.FlagRole == 1 );
                var countCaptain = _dbContext.Registration.CountAsync(u => u.FlagRole == 2);
                var totalCount = 0;

                if (countCaptain.Result > 0)
                {
                    totalCount = countPlayer.Result + 1;
                }

                if (countCaptain.Result == 0)
                {
                    totalCount = countPlayer.Result;
                }

                return totalCount;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public DashboardDTO getCaptain()
        {
            try
            {
                var captainNameDb = _dbContext.Registration
                                      .Where(u => u.FlagRole == 2)
                                      .Select(u => new { u.FirstName, u.Email })
                                      .FirstOrDefault();

                DashboardDTO dashboard = new DashboardDTO()
                {
                    captainName =  captainNameDb.FirstName,
                    captainEmail = captainNameDb.Email
                };

                return dashboard;
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);

            }
        }

        public List<DashboardDTO> getPlayers()
        {
            try
            {
                var players = _dbContext.Registration
                              .Where(u => u.FlagRole == 1 || u.FlagRole == 2 )
                              .Select(u => new {u.Email , u.FirstName})
                              .ToList();
                
                var playerList = new List<DashboardDTO>();

                foreach ( var player in players )
                {
                    var dashboardDTO = new DashboardDTO()
                    {
                        playerEmail = player.Email ,
                        playerName = player.FirstName
                    };
                    playerList.Add(dashboardDTO);
                }

                return playerList;
            }
            catch(Exception ex )
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<DashboardDTO> getCoach()
        {
            try
            {
                var coachNameDb = _dbContext.Registration
                                  .Where(u => u.FlagRole == 5)
                                  .Select(u => new { u.FirstName, u.Email })
                                  .FirstOrDefault();
                if (coachNameDb != null)
                {
                    DashboardDTO dashboard = new DashboardDTO()
                    {
                        coachName = coachNameDb.FirstName,
                        coachEmail = coachNameDb.Email
                    };

                    return dashboard;
                }
                return null;
                
            }
            catch(Exception ex) 
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
            
        }
    }
}
