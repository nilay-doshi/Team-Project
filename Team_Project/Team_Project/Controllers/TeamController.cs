using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Team_Project.Models;
using Team_Project.Repository;

namespace Team_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public TeamController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("CreateTeamPlayers")]
        public async Task<IActionResult> CreateTeamPlayers(CreateTeamDTO team)
        {
            if (team == null || team.coachEmail == null || team.playersEmail == null)
            {
                return BadRequest();
            }

            if (team.coachEmail == "nilaydoshi@gmail.com")
            {
                var players = await _userRepository.SavePlayers(team.playersEmail);
                Console.WriteLine("Players made successfully now create for captain");
                if (players == null)
                {
                    return BadRequest();
                }
                
                return Ok(players);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("CreateCaptain")]
        public IActionResult CreateCaptain(CreateTeamDTO team)
        {
            if (team == null || team.coachEmail == null || team.captainEmail == null)
            {
                return BadRequest();
            }

            if (team.coachEmail == "nilaydoshi@gmail.com")
            {
                var captain = _userRepository.SaveCaptain(team.captainEmail);
                if (captain == null)
                {
                    return BadRequest();
                }
                return Ok(captain);
            }
            return BadRequest();
        }


        [HttpPost("CreateTeamByCaptain")]
        public  IActionResult CreateTeamByCaptain(CreateTeamDTO team) 
        {
            var verifyCaptain = _userRepository.verifyCaptain(team.captainEmail);
            if (verifyCaptain == null)
            {
                return BadRequest(" Sorry You are not a authorized person");
            }
            int getCount = _userRepository.getPlayerCount();
            if (getCount < 5 ) {

                _userRepository.SavePlayersByCaptain(team.playersEmail);
              //  return Ok(getCount);
            }
            int getCount1 = _userRepository.getPlayerCount();
            
            return Ok("Total Players "+getCount1);
        }

    }
}
