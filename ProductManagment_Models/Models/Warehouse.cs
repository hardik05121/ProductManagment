using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Warehouse
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string WarehouseName { get; set; } = null!;

    public long? ContactPerson { get; set; }

    public long? MobileNumber { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();
}
