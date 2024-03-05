using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Team.Service.DTO
{
    public class CreateTeamDTO
    {
        [JsonIgnore]
        public string coachEmail { get; set; } = "nilaydoshi@gmail.com";
        public string? captainEmail { get; set; }

        [Required]
        [Length(1, 15)]
        public string[]? playersEmail { get; set; } 
    }
}
