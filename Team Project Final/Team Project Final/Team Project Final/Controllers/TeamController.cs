using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Team.Repo.Interface;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team_Project_Final.Controllers
{
    public class TeamController : BaseController
    {
        private readonly ITeamService _teamService;

        #region constructor region
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        #endregion

        #region Create team by coach api
        [Authorize(Roles = "5")]
        [HttpPost("CreateTeamByCoach")]
        public async Task<IActionResult> CreateTeamByCoach(CreateTeamDTO teamdto)
        {
            try
            {
                var players = await _teamService.SavePlayers(teamdto);

                Console.WriteLine("Players made successfully now create for captain");
                if (players == null)
                {
                    return BadRequest();
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region Create Captain
        [Authorize(Roles = "5")]
        [HttpPost("CreateCaptain")]
        public async Task<IActionResult> CreateCaptain(CreateTeamDTO teamdto)
        {
            try
            {
                var captain = await _teamService.SaveCaptain(teamdto);
                if (captain == null)
                {
                    return BadRequest(captain);
                }
                return Ok(captain);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region Create Team by Captain Api

        [Authorize(Roles = "2")]
        [HttpPost("CreateTeamByCaptain")]
        public async Task<IActionResult> CreateTeamByCaptain(CreateTeamDTO teamdto)
        {
            try
            {
                if (teamdto.playersEmail.Length < 11)
                {
                    var players = await _teamService.SavePlayers(teamdto);
                    return Ok(players);
                }
                return BadRequest("Enter number of players less then 11 ");
            }

            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region Get Captain Detail

        [Authorize(Roles = "1,5")]
        [HttpGet("getCaptainDashboard")]
        public async Task<IActionResult> getCaptainDashboard()
        {
            var user = await _teamService.getCaptain();
            return Ok(user);
        }
        #endregion

        #region Get Players Detail 

        [Authorize(Roles = "2,5")]
        [HttpGet("getAllPlayersDashboard")]
        public async Task<IActionResult> getPlayersDashboard()
        {
            var users = await _teamService.getPlayers();
            return Ok(users);
        }
        #endregion

        #region Get Coach Api
        [Authorize(Roles = "1,5")]
        [HttpGet("getCoachDashboard")]
        public async Task<IActionResult> getCoachDashboard()
        {
            var user = await _teamService.getCoach();
            return Ok(user);
        }
        #endregion

    }
}
