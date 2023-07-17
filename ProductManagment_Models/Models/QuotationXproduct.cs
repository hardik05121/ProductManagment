using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[Table("QuotationXProducts")]
public partial class QuotationXproduct
{
    [Key]
    public int Id { get; set; }

    public int UnitId { get; set; }

    public int QuotationId { get; set; }

    public int WarehouseId { get; set; }
   
    public int ProductId { get; set; }

    public int TaxId { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public double? Subtotal { get; set; }

    public double? Discount { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("QuotationXproducts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("QuotationId")]
    [InverseProperty("QuotationXproducts")]
    public virtual Quotation Quotation { get; set; } = null!;

    [ForeignKey("TaxId")]
    [InverseProperty("QuotationXproducts")]
    public virtual Tax Tax { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("QuotationXproducts")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("QuotationXproducts")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
