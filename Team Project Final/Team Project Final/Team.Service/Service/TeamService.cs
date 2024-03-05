using Team.Repo.Interface;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team.Service.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Task Savecaptain(CreateTeamDTO teamdto)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseDTO> SavePlayers(CreateTeamDTO teamdto)
        {
            if (teamdto.coachEmail == "nilaydoshi@gmail.com")
            {
                var Checkplayerscount = await _teamRepository.checkPlayercount();
                var Checkcaptaincount = await _teamRepository.checkCaptaincount();
                var afteraddCount = 0;
                int playerCount = int.Parse(Checkplayerscount);
                int captaincount = int.Parse(Checkcaptaincount);
                if (captaincount > 0)
                {
                    afteraddCount = playerCount + teamdto.playersEmail.Length + 1;
                }

                if (captaincount == 0)
                {
                    afteraddCount = playerCount + teamdto.playersEmail.Length;
                }

                if (playerCount > 14 || afteraddCount > 15)
                {
                    return new ResponseDTO
                    {
                        Status = 500,
                        Message = "Error"
                    };
                }
                var players = await _teamRepository.SavePlayers(teamdto.playersEmail);

                Console.WriteLine("Players made successfully now you can create 1 captain");
                if (players == null)
                {
                    return new ResponseDTO
                    {
                        Status = 500,
                        Message = "Error"
                    };
                }
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Success",
                };
            }
            return new ResponseDTO
            {
                Status = 500,
                Message = "Coach email cannot be verifies"
            };
        }

        public async Task<ResponseDTO> SaveCaptain(CreateTeamDTO teamDTO)
        {
            var checkCaptaincount = await _teamRepository.checkCaptaincount();
           var captaincount = int.Parse(checkCaptaincount);

            if (captaincount != 0)
            {
                return new ResponseDTO { Status = 500, Message = "Captain already exists." };
            }

            var makecaptain = await _teamRepository.SaveCaptain(teamDTO.captainEmail);
            return new ResponseDTO { Status = 200, Data = makecaptain, Message = "User created captain successfully" };

        }

        public async Task<ResponseDTO> getCaptain()
        {
            try
            {
                var user = await _teamRepository.getCaptain();
                var user1 = " Name : " + user.captainName + "Email" + user.captainEmail;
                return new ResponseDTO { Status = 200, allData = user1 };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);

            }
        }

        public async Task<ResponseDTO> getPlayers()
        {
            try
            {
                var users = await _teamRepository.getallPlayers();
                var dashboardUsers = users.Select(user => $"{user.playerEmail} {user.playerName}").ToList();
                var finalplayers = string.Join(", ", dashboardUsers);

                return new ResponseDTO
                {
                    Status = 200,
                    allData = finalplayers
                };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);

            }
        }

        public async Task<ResponseDTO> getCoach()
        {
            try
            {
                var user = await _teamRepository.getCoach();
                return new ResponseDTO { allData = user.coachEmail + " " +user.coachName };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
    }
}
