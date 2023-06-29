using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Country
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string CountryName { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [InverseProperty("Country")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("Country")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    [InverseProperty("Country")]
    public virtual ICollection<State> States { get; set; } = new List<State>();

    [InverseProperty("BillingCountry")]
    public virtual ICollection<Supplier> SupplierBillingCountries { get; set; } = new List<Supplier>();

    [InverseProperty("Country")]
    public virtual ICollection<Supplier> SupplierCountries { get; set; } = new List<Supplier>();

    [InverseProperty("ShippingCountry")]
    public virtual ICollection<Supplier> SupplierShippingCountries { get; set; } = new List<Supplier>();
}
