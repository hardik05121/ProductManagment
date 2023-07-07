using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[Table("companies")]
public partial class Company
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string Title { get; set; } = null!;

    [StringLength(450)]
    public string Currency { get; set; } = null!;

    [StringLength(450)]
    public string? Address { get; set; }

    public long? PhoneNumber { get; set; }

    [StringLength(450)]
    public string? Email { get; set; }

    public bool IsActive { get; set; }

    [StringLength(450)]
    public string? CompanyImage { get; set; }
}
