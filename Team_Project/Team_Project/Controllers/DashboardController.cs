using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team_Project.Models;
using Team_Project.Repository;

namespace Team_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public DashboardController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "1,5")]
        // [AllowAnonymous]
        [HttpGet("getCaptainDashboard")]
        public string getCaptainDashboard()
        {
            var user = _userRepository.getCaptain();
            var user1 = " Name : " + user.captainName + "Email" + user.captainEmail;
            return user1;
        }

        [Authorize(Roles = "2,5")] 
        [HttpGet("getAllPlayersDashboard")]
        public List<string> getPlayersDashboard()
        {
            var users = _userRepository.getPlayers();
            var dashboardUsers = users.Select(user => $"{user.playerEmail} {user.playerName}" ).ToList();
            return dashboardUsers;
        }

        [Authorize(Roles = "1")]
        [HttpGet("getCoachDashboard")]
        public async Task<string> getCoachDashboard()
        {
            var user = await _userRepository.getCoach();
            return user.coachName;
        }


    }
}
