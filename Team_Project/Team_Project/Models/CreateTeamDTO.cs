using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team_Project.Models
{
    public class CreateTeamDTO
    {
        //  [Required]
        [JsonIgnore]
        public string coachEmail { get; set; } = "nilaydoshi@gmail.com";


            
        public string? captainEmail { get; set; }

        [Required]
        [Length(1,15)]
        public string[]? playersEmail { get; set; }


    }
}
