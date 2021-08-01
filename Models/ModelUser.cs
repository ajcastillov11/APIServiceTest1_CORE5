using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceAPITest.Models
{
    public class ModelUser
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(255)]
        [Required]
        public string UserName { get; set; }
        [StringLength(255)]
        [MinLength(5)]
        [Required]
        public string Password { get; set; }

    }
}
