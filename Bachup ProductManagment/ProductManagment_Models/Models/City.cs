using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class City
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string CityName { get; set; } = null!;

    public int StateId { get; set; }

    public int CountryId { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("Cities")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("City")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    [ForeignKey("StateId")]
    [InverseProperty("Cities")]
    public virtual State State { get; set; } = null!;

    [InverseProperty("BillingCity")]
    public virtual ICollection<Supplier> SupplierBillingCities { get; set; } = new List<Supplier>();

    [InverseProperty("City")]
    public virtual ICollection<Supplier> SupplierCities { get; set; } = new List<Supplier>();

    [InverseProperty("ShippingCity")]
    public virtual ICollection<Supplier> SupplierShippingCities { get; set; } = new List<Supplier>();
}
