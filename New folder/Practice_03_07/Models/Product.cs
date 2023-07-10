using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Models
{
    public class Product
    {
        [Key]

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        
        public string Image { get; set; }

        [Required]
        public string ShortDsc { get; set; }

        public int CatId { get; set; }

        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }
    }
}
