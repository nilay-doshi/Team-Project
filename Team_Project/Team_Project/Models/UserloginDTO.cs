using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team_Project.Models
{
    public class UserloginDTO
    {
        [EmailAddress(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        public string Password { get; set; }
       // public string? FlagRole {  get; set; }
    }
}
