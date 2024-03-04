using System.Text.Json.Serialization;

namespace Team_Project.Models
{
    public class DashboardDTO
    {
        [JsonIgnore]
        public string? captainName {  get; set; }

        [JsonIgnore]
        public string? captainEmail { get; set; }

        [JsonIgnore]
        public string? coachName { get; set; } = "Nilay";

        [JsonIgnore]
        public string? coachEmail { get; set; } = "nilaydoshi@gmail.com";

        [JsonIgnore]
        public string? playerName { get; set; }

        [JsonIgnore]
        public string? playerEmail { get; set;}
    }
}
