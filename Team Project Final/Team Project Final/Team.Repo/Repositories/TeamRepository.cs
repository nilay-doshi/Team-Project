using Microsoft.EntityFrameworkCore;
using Team.Repo.DTO;
using Team.Repo.Enums;
using Team.Repo.Interface;
using Team.Repo.Models;

namespace Team.Repo.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        #region Fields
        private readonly TeamDBContext _dbContext;
        static int enumUserId = (int)RoleEnum.User;
        static int enumCaptainId = (int)RoleEnum.Captain;
        static int enumPlayerId = (int)RoleEnum.Player;
        static int enumCoachId = (int)RoleEnum.Coach;
        #endregion

        #region Constructor
        public TeamRepository(TeamDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        #endregion

        #region CheckCaptain Count
        public async Task<string> checkCaptaincount()
        {
            var Checkcaptaincount = await _dbContext.Registration
                                    .CountAsync(u => u.FlagRole == enumCaptainId);
            return Checkcaptaincount.ToString();

        }
        #endregion

        #region CheckPlayer Count
        public async Task<string> checkPlayercount()
        {
            var Checkplayerscount = await _dbContext.Registration
                                       .CountAsync(u => u.FlagRole == enumPlayerId);
            return Checkplayerscount.ToString();
        }
        #endregion

        #region Save Players
        public async Task<List<UserRegistration>> SavePlayers(string[] playersEmail)
        {
            var users = await _dbContext.Registration
                               .Where(u => playersEmail.Contains(u.Email) && u.FlagRole == enumUserId)
                               .ToListAsync();

            foreach (var user in users)
            {
                user.FlagRole = enumPlayerId;
            }
            _dbContext.SaveChanges();
            foreach (var user in users)
            {
                user.Password = null;
            }
            return users;
        }
        #endregion

        #region Save Captain
        public async Task<UserRegistration?> SaveCaptain(string captainEmail)
        {
            try
            {
                var makeCaptain = await _dbContext.Registration
                          .FirstOrDefaultAsync(u => u.Email == captainEmail && u.FlagRole == enumPlayerId);
                if (makeCaptain != null)
                {
                    makeCaptain.FlagRole = enumCaptainId;
                   await _dbContext.SaveChangesAsync();
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
        #endregion

        #region Get Captain

        public async Task<DashBoardDTO> getCaptain()
        {
            try
            {
                var captainNameDb = await _dbContext.Registration
                                     .Where(u => u.FlagRole == enumCaptainId)
                                     .Select(u => new { u.FirstName, u.Email })
                                     .FirstOrDefaultAsync();

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
        #endregion

        #region Get All Players
        public async Task<List<DashBoardDTO>> getallPlayers()
        {
            try
            {
                var players =  await _dbContext.Registration
                              .Where(u => u.FlagRole == enumPlayerId || u.FlagRole == enumCaptainId)
                              .Select(u => new { u.Email, u.FirstName })
                              .ToListAsync();

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
        #endregion

        #region Get Coach
        public async Task<DashBoardDTO> getCoach()
        {
            try
            {
                var coachNameDb = await _dbContext.Registration
                                  .Where(u => u.FlagRole == enumCoachId)
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
        #endregion
    }
}
