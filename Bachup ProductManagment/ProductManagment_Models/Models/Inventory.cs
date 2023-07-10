using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Inventory
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public double Stock { get; set; }

    public int UnitId { get; set; }

    public double UnitPrice { get; set; }

    public int WarehouseId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Inventories")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Inventories")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("Inventories")]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
