using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Subcategory
    {
        [Key]
        public int Sid { get; set; }

        [Required(ErrorMessage ="Subcategory is required")]
        [MaxLength(50,ErrorMessage ="Maximum character is not more than 50 ")]

        public string Name { get; set; }

        public int CatId { get; set; }


    }
}
