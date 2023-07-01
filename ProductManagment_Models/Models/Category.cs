using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [MaxLength(10, ErrorMessage = "Name must be 10 characters or less")]
    public string Name { get; set; } = null!;

    [StringLength(450)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
