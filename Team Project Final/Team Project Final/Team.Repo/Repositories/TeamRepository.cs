using Microsoft.EntityFrameworkCore;
using Team.Repo.DTO;
using Team.Repo.Interface;
using Team.Repo.Models;

namespace Team.Repo.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamDBContext _dbContext;
        public TeamRepository(TeamDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<string> checkCaptaincount()
        {
            var Checkcaptaincount = await _dbContext.Registration
                                       .CountAsync(u => u.FlagRole == 2);
            return Checkcaptaincount.ToString();

        }
        public async Task<string> checkPlayercount()
        {
            var Checkplayerscount = await _dbContext.Registration
                                       .CountAsync(u => u.FlagRole == 1);
            return Checkplayerscount.ToString();
        }
        public async Task<List<UserRegistration>> SavePlayers(string[] playersEmail)
        {
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
        public async Task<UserRegistration?> SaveCaptain(string captainEmail)
        {
            try
            {
                var makeCaptain = _dbContext.Registration
                          .FirstOrDefault(u => u.Email == captainEmail && u.FlagRole == 1);
                if (makeCaptain != null)
                {
                    makeCaptain.FlagRole = 2;
                    _dbContext.SaveChanges();
                    makeCaptain.Password = null;
                }

                return makeCaptain;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<DashBoardDTO> getCaptain()
        {
            try
            {
                var captainNameDb = _dbContext.Registration
                                     .Where(u => u.FlagRole == 2)
                                     .Select(u => new { u.FirstName, u.Email })
                                     .FirstOrDefault();

                DashBoardDTO dashboard = new DashBoardDTO()
                {
                    captainName = captainNameDb.FirstName,
                    captainEmail = captainNameDb.Email
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<List<DashBoardDTO>> getallPlayers()
        {
            try
            {
                var players = _dbContext.Registration
                              .Where(u => u.FlagRole == 1 || u.FlagRole == 2)
                              .Select(u => new { u.Email, u.FirstName })
                              .ToList();

                var playerList = new List<DashBoardDTO>();

                foreach (var player in players)
                {
                    var dashboardDTO = new DashBoardDTO()
                    {
                        playerEmail = player.Email,
                        playerName = player.FirstName
                    };
                    playerList.Add(dashboardDTO);
                }

                return playerList;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }

        public async Task<DashBoardDTO> getCoach()
        {
            try
            {
                var coachNameDb = await _dbContext.Registration
                                  .Where(u => u.FlagRole == 5)
                                  .Select(u => new { u.FirstName, u.Email })
                                  .FirstOrDefaultAsync();
                if (coachNameDb != null)
                {
                    DashBoardDTO dashboard = new DashBoardDTO()
                    {
                        coachName = coachNameDb.FirstName,
                        coachEmail = coachNameDb.Email
                    };

                    return dashboard;
                }
                return null;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
    }
}
