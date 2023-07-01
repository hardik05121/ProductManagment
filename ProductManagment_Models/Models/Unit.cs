﻿using System;
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
    [Display(Name = "UnitName*")]
    public string UnitName { get; set; } = null!;

    [StringLength(50)]
    public string? BaseUnit { get; set; }

    [Display(Name = "Add Your UnitCode")]
    public int? UnitCode { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Unit")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
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