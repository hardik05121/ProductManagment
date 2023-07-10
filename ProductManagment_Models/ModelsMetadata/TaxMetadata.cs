using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(TaxMetadata))]
public partial class Tax
{

}

public partial class TaxMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "TaxName*")]
    public string Name { get; set; } = null!;

    [Display(Name = "Enter Percentage")]
    public double? Percentage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

