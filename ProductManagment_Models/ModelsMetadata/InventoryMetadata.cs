using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(InventoryMetadata))]
public partial class Inventory
{

}

public partial class InventoryMetadata
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public double Stock { get; set; }

    public int UnitId { get; set; }

    public double UnitPrice { get; set; }

    public int WarehouseId { get; set; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UnitId")]
    [ValidateNever]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [ValidateNever]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
