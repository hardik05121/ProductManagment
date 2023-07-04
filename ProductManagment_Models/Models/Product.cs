using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;


namespace ProductManagment_Models.Models;

public partial class Product
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
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Brand Brand { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    [InverseProperty("Product")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [ForeignKey("TaxId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Tax Tax { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("Products")]
    [ValidateNever]
    public virtual Warehouse Warehouse { get; set; } = null!;
}



//worked string;-

//[Display(Name = "Price for 50+")]

//[Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]

//[StringLength(60, MinimumLength = 3)]

//[DataType(DataType.Date)]
//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]

//[DataType(DataType.PhoneNumber)]
//[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
//[StringLength(13, MinimumLength = 10)]
//[RegularExpression(@"^(0|91)?[6-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]
//[RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Invalid Mobile Number.")]

//[StringLength(50)]
//[DataType(DataType.EmailAddress)]
//[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
//public string? Email { get; set; }

//[Required(ErrorMessage = "Please enter the product URL.")]
//[RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
//    ErrorMessage = "Please enter a valid URL.")]
//public string? WebSite { get; set; }

//[Display(Name = "Joining Date")]
//[DataType(DataType.Date)]
//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
//public DateTime JoiningDate { get; set; }