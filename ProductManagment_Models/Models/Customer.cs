using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Customer
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string CustomerName { get; set; } = null!;

    [StringLength(50)]
    public string? ContactPerson { get; set; }

    [StringLength(450)]
    public string Email { get; set; } = null!;

    public long MobileNumber { get; set; }

    public long? PhoneNumber { get; set; }

    [StringLength(50)]
    public string? WebSite { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    [StringLength(450)]
    public string? Description { get; set; }

    [StringLength(450)]
    public string? CustomerImage { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Customers")]
    public virtual City City { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("Customers")]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("StateId")]
    [InverseProperty("Customers")]
    public virtual State State { get; set; } = null!;
}
