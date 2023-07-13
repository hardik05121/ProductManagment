using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[Table("PurChaseOrderXProducts")]
public partial class PurChaseOrderXproduct
{
    [Key]
    public int Id { get; set; }

    public int PurChaseOrderId { get; set; }

    public int WarehouseId { get; set; }

    public int ProductId { get; set; }

    public int UnitId { get; set; }

    public int TaxId { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public double? Subtotal { get; set; }

    public double? Discount { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("PurChaseOrderXproducts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("PurChaseOrderId")]
    [InverseProperty("PurChaseOrderXproducts")]
    public virtual PurChaseOrder PurChaseOrder { get; set; } = null!;

    [ForeignKey("TaxId")]
    [InverseProperty("PurChaseOrderXproducts")]
    public virtual Tax Tax { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("PurChaseOrderXproducts")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("PurChaseOrderXproducts")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
