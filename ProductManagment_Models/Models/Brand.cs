using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Brand
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string BrandName { get; set; } = null!;

    [StringLength(450)]
    public string? BrandImage { get; set; }

    [InverseProperty("Brand")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
