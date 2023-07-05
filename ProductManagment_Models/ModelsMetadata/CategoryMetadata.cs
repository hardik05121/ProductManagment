using Microsoft.AspNetCore.Mvc;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_Models.Models
{
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {

    }

    //[Bind(Exclude = "ID")]
    public partial class CategoryMetadata
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Category Name*")]
        [MaxLength(10, ErrorMessage = "Name must be 10 characters or less")]
        public string Name { get; set; } = null!;

        [StringLength(450)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }


}
