using Team.Repo.Enums;
using Team.Repo.Interface;
using Team.Service.DTO;
using Team.Service.Interface;

namespace Team.Service.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
     //   static int enumUserId = (int)RoleEnum.User;
        static int enumCoachcount = (int)CountEnum.CoachMaxCount;
        static int enumCaptaincount = (int)CountEnum.CaptainMaxCount;
        static int enumtotalCaptain = (int)CountEnum.TotalCaptain;
        static int enumtotalPlayers = (int)CountEnum.TotalPlayers;

        #region Constructor
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        #endregion

        #region Save Players
        public async Task<ResponseDTO> SavePlayers(CreateTeamDTO teamdto)
        {
            if (teamdto.coachEmail == "nilaydoshi@gmail.com")
            {
                var Checkplayerscount = await _teamRepository.checkPlayercount();
                var Checkcaptaincount = await _teamRepository.checkCaptaincount();
                int afteraddCount = 0;
                int playerCount = int.Parse(Checkplayerscount);
                int captaincount = int.Parse(Checkcaptaincount);
                if (captaincount == enumtotalCaptain)
                {
                    afteraddCount = playerCount + teamdto.playersEmail.Length + 1;
                }

                if (captaincount < enumtotalCaptain)
                {
                    afteraddCount = playerCount + teamdto.playersEmail.Length;
                }

                if (playerCount > enumtotalPlayers || afteraddCount > enumCoachcount)
                {
                    var afteraddCount1 = afteraddCount - 1;
                    return new ResponseDTO
                    {
                        Status = 403,
                        Message = "add less number of players. Current number of players in team are "+afteraddCount1
                    };
                }
                var players = await _teamRepository.SavePlayers(teamdto.playersEmail);

                Console.WriteLine("Players made successfully now you can create 1 captain");
                if (players == null)
                {
                    return new ResponseDTO
                    {
                        Status = 500,
                        Message = "Internal Server Error"
                    };
                }
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Success"
                };
            }
            return new ResponseDTO
            {
                Status = 401,
                Message = "Coach email cannot be verifies"
            };
        }
        #endregion

        #region Save Captain

        public async Task<ResponseDTO> SaveCaptain(CreateTeamDTO teamDTO)
        {
            var checkCaptaincount = await _teamRepository.checkCaptaincount();
           var captaincount = int.Parse(checkCaptaincount);

            if (captaincount != 0)
            {
                return new ResponseDTO { Status = 400, Message = "Captain already exists." };
            }

            var makecaptain = await _teamRepository.SaveCaptain(teamDTO.captainEmail);
            return new ResponseDTO { Status = 200, Data = makecaptain, Message = "User created captain successfully" };

        }
        #endregion

        #region GetCaptainDetail

        public async Task<ResponseDTO> getCaptain()
        {
            try
            {
                var user = await _teamRepository.getCaptain();
               // var userName =  user.captainName; 
              //  var userEmail = user.captainEmail;
                return new ResponseDTO { Status = 200, allData ="Name : "+ user.captainName + " Email : " + user.captainEmail };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        #region GetPlayers Detail

        public async Task<ResponseDTO> getPlayers()
        {
            try
            {
                var users = await _teamRepository.getallPlayers();
                var playerDetails = users.Select(user => $"Email: {user.playerEmail}, Name: {user.playerName}");
                var finalPlayers = string.Join(", ", playerDetails);

                return new ResponseDTO
                {
                    Status = 200,
                    allData = finalPlayers
                };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);

            }
        }
        #endregion

        #region GetCoachDetail

        public async Task<ResponseDTO> getCoach()
        {
            try
            {
                var user = await _teamRepository.getCoach();
                return new ResponseDTO { allData = " Email : " + user.coachEmail + " , Name " +user.coachName ,Status = 200 , Message = "Success", Error = "null" };
            }
            catch (Exception ex)
            {   
                string errorMessage = ex.Message;
                throw new NotImplementedException(errorMessage);
            }
        }
        #endregion

        //public async Task<string> getGame()
        //{
        //    Console.WriteLine("Hello from interface. ");
        //    return null;
        //}
    }
}
