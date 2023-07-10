using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Tax
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public double? Percentage { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Tax")]
    public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

    [InverseProperty("Tax")]
    public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();
}
