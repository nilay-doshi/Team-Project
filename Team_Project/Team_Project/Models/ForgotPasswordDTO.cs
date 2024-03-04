using System.ComponentModel.DataAnnotations;

namespace Team_Project.Models
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string newPassword { get; set; }
    }
}
