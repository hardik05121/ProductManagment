using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(WarehouseMetadata))]
public partial class Warehouse
{

}

public partial class WarehouseMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Display(Name = "WarehouseName*")]
    public string WarehouseName { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "ContactPerson*")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number And Enter a number For 10 digit.")]
    public long? ContactPerson { get; set; }


    public long? MobileNumber { get; set; }

    [StringLength(50)]
    [DataType(DataType.EmailAddress)]

    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

}