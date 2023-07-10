using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
   
    
        public class Product
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Product Name should not empty")]
            [MaxLength(100, ErrorMessage = "product name should  not more than 100")]
            public string Name { get; set; }

            [Range(1, Int32.MaxValue, ErrorMessage = "Qty should be more than 0")]
            public int Qty { get; set; }

            [Range(1, Int32.MaxValue, ErrorMessage = "Rate should be more than 0")]

            public int Rate { get; set; }
            public string Profile { get; set; }

            public bool IsActive { get; set; } = true;
            public int CatId { get; set; }

            //[NotMapped]
            //public string FileData { get; set; }
  
            [NotMapped]
            public IFormFile Image { get; set; }


        }
    }



