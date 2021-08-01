using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceAPITest.Models
{
    public class ModelProduct
    {
        [Key]
        public int ProductId { get; set; }
        [StringLength(255)]
        [Required]
        [MinLength(3)]
        public string ProductName { get; set; }
        [StringLength(255)]
        public string IntroductionDate { get; set; }
        [StringLength(100)]
        public string Price { get; set; }
        [StringLength(400)]
        public string Url { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public ModelCategory Category { get; set; }
    }
}
