﻿using Microsoft.AspNetCore.Http.HttpResults;
using Team.Service.DTO;

namespace Team.Service.Interface
{
    public interface ITeamService
    {
        public Task<ResponseDTO> SavePlayers(CreateTeamDTO teamdto);
        public Task<ResponseDTO> SaveCaptain(CreateTeamDTO teamDTO);
        public Task<ResponseDTO> getCaptain();
        public Task<ResponseDTO> getPlayers();
        public Task<ResponseDTO> getCoach();

        void  getGame()
        {
            Console.WriteLine("Hello from interface. ");
        }
    }
}
