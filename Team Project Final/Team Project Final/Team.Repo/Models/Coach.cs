using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Repo.Models
{
    public class Coach
    {
       [ DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Coachid { get; set; }

        [EmailAddress(ErrorMessage = "Email address is required")]
        [Key]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "FirstName is compulsory")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "LastName is compulsory")]
        public string LastName { get; set; } = null!;
        [Phone(ErrorMessage = "Contact Number is compulsory")]
        public string ContactNumber { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public int? FlagRole { get; set; } = 5;
    }
}
