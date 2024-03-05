using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team.Repo.Models;

namespace Team.Service.DTO
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public UserRegistration? Data { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }

        public string? token { get; set; }

        public string? allData { get; set; }
    }
}
