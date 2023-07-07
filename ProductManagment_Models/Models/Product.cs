using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Code { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public int UnitId { get; set; }

    public int WarehouseId { get; set; }

    public int TaxId { get; set; }

    [StringLength(50)]
    public string? SkuCode { get; set; }

    [StringLength(50)]
    public string? SkuName { get; set; }

    public double? SalesPrice { get; set; }

    public double? PurchasePrice { get; set; }

    [Column("MRP")]
    public double? Mrp { get; set; }

    public long? BarcodeNumber { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    [StringLength(450)]
    public string? ProductImage { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual Brand Brand { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    [InverseProperty("Product")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Product")]
    public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

    [InverseProperty("Product")]
    public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();

    [ForeignKey("TaxId")]
    [InverseProperty("Products")]
    public virtual Tax Tax { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Products")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("Products")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
