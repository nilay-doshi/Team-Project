using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team_Project.Models;

namespace Team_Project.Repository
{
    public interface IUserRepository
    {
      public Task<UserRegistration> AddUser(UserRegistration userRegistration);
      public Task<ActionResult<string>> GetUserByEmail(string email, string Password);
      public ActionResult<string> GetUserByEmail1(string email);
      public string updatePassword(string email, string passoword);
      public Task<List<UserRegistration>> SavePlayers(string[] playersEmail);
      public Task<List<UserRegistration?>> SavePlayersByCaptain(string[] playersEmail);
      public UserRegistration? SaveCaptain(string captainEmail);
      public UserRegistration? verifyCaptain(string captainEmail);
      public int getPlayerCount();
      public DashboardDTO getCaptain();
      public List<DashboardDTO> getPlayers();
      public Task<DashboardDTO> getCoach();
    }
}
