using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class State
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string StateName { get; set; } = null!;

    public int CountryId { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("CountryId")]
    [InverseProperty("States")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("State")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("State")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    [InverseProperty("BillingState")]
    public virtual ICollection<Supplier> SupplierBillingStates { get; set; } = new List<Supplier>();

    [InverseProperty("ShippingState")]
    public virtual ICollection<Supplier> SupplierShippingStates { get; set; } = new List<Supplier>();

    [InverseProperty("State")]
    public virtual ICollection<Supplier> SupplierStates { get; set; } = new List<Supplier>();
}
