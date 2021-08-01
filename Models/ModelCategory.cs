using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceAPITest.Models
{
    public class ModelCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(255)]
        [Required]
        public string CategoryName { get; set; }

    }
}
