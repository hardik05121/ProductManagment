using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Unit
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string UnitName { get; set; } = null!;

    [StringLength(50)]
    public string? BaseUnit { get; set; }

    public int? UnitCode { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Unit")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Unit")]
    public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

    [InverseProperty("Unit")]
    public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();
}
