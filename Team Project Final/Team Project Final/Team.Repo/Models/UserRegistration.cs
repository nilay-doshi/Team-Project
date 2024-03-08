using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Team.Repo.Enums;

namespace Team.Repo.Models
{
    public class UserRegistration
    {
        static int enumcount = (int)CountEnum.FlagCount;
        static int enumUserId = (int)RoleEnum.User;


        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is required")]
        [Key]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [JsonIgnore]
        public string? Password { get; set; }

        [Required(ErrorMessage = "FirstName is compulsory")]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is compulsory")]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone(ErrorMessage = "Contact Number is compulsory")]
        [MaxLength(50)]
        public string ContactNumber { get; set; } = null!;

        [Required]
        [DateNotGreaterThanToday(ErrorMessage = "Date of birth cannot be greater than today.")]
        public DateOnly Dob { get; set; }

        [JsonIgnore]
        public int? FlagRole { get; set; } = enumUserId;
        [JsonIgnore]
        public int? FlagCouunt { get; set; } = enumcount;

    }
    public class DateNotGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly dob)
            {
                if (dob > DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult("Date of birth cannot be greater than today.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
