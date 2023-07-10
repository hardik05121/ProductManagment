using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(UnitMetadata))]
public partial class Unit
{
}

public partial class UnitMetadata
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

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
