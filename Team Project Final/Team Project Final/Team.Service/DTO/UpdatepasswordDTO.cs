using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team.Service.DTO
{
    public class UpdatepasswordDTO
    {
        [Required]
        public string newPassword { get; set; }
    }
}
