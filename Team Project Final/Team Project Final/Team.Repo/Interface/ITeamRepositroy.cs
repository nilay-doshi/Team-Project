using Team.Repo.DTO;
using Team.Repo.Models;

namespace Team.Repo.Interface
{
    public interface ITeamRepository
    {
        Task<string> checkCaptaincount();
        Task<string> checkPlayercount();
        Task<List<UserRegistration>> SavePlayers(string[] playersEmail);
        Task<UserRegistration?> SaveCaptain(string captainEmail);
        Task<DashBoardDTO> getCaptain();
        Task<List<DashBoardDTO>> getallPlayers();
        Task<DashBoardDTO> getCoach();
    }
}
