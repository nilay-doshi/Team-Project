using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Team.Repo.DTO
{
    public class DashBoardDTO
    {
         [JsonIgnore]
            public string? captainName { get; set; }

            [JsonIgnore]
            public string? captainEmail { get; set; }

            [JsonIgnore]
            public string? coachName { get; set; } = "Nilay";

            [JsonIgnore]
            public string? coachEmail { get; set; } = "nilaydoshi@gmail.com";

            [JsonIgnore]
            public string? playerName { get; set; }

            [JsonIgnore]
            public string? playerEmail { get; set; }
    }
}
