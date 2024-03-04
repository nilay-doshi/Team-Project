using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team_Project.Models
{
    public class UserRegistration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [EmailAddress(ErrorMessage = "Email address is required")]
        [Key]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is compulsory")]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is compulsory")]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Phone(ErrorMessage = "Contact Number is compulsory")]
        [MaxLength(50)]
        public string ContactNumber { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public int? FlagRole { get; set; } = 1;

        public int? FlagCouunt { get; set; } = 0;
    }
}
