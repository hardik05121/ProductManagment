using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;


namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(ProductMetadata))]
public partial class Product
{

}
public partial class ProductMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "ProductName*")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Display(Name = "Enter Product Code*")]
    public string? Code { get; set; }

    [Display(Name = "Select Brand*")]
    public int BrandId { get; set; }

    [Display(Name = "Select Category*")]
    public int CategoryId { get; set; }

    [Display(Name = "Select Unit*")]
    public int UnitId { get; set; }

    [Display(Name = "WareHouse*")]
    public int WarehouseId { get; set; }

    [Display(Name = "Select Tax*")]
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

    [StringLength(50, MinimumLength = 5)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Display(Name = "Created Date*")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Updated Date*")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedDate { get; set; }

    [StringLength(450)]

    public string? ProductImage { get; set; }

    [ForeignKey("BrandId")]
  
    public virtual Brand Brand { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [ValidateNever]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [ForeignKey("TaxId")]
    [ValidateNever]
    public virtual Tax Tax { get; set; } = null!;

    [ForeignKey("UnitId")]
    [ValidateNever]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [ValidateNever]
    public virtual Warehouse Warehouse { get; set; } = null!;
}

