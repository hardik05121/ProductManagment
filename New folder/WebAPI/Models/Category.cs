using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name Should not be empty")]
        [MaxLength(100, ErrorMessage = "Category name should not be more than 100")]
        public string Name { get; set; }
    }
}
