using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(CompanyMetadata))]
public partial class Company
{

}

[Table("companies")]
public partial class CompanyMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]

    public string Title { get; set; } = null!;

    [StringLength(450)]
    public string Currency { get; set; } = null!;

    [StringLength(450)]
    public string? Address { get; set; }

    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
    public long? PhoneNumber { get; set; }
    [StringLength(450)]
    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public bool IsActive { get; set; }

    [StringLength(450)]
    public string? CompanyImage { get; set; }
}
