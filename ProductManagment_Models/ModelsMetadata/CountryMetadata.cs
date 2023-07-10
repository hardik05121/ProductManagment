using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(CountryMetadata))]
public partial class Country
{

}

public partial class CountryMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string CountryName { get; set; } = null!;

    public bool IsActive { get; set; }


    public virtual ICollection<City> Cities { get; set; } = new List<City>();


    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();


    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();


    public virtual ICollection<State> States { get; set; } = new List<State>();


    public virtual ICollection<Supplier> SupplierBillingCountries { get; set; } = new List<Supplier>();


    public virtual ICollection<Supplier> SupplierCountries { get; set; } = new List<Supplier>();


    public virtual ICollection<Supplier> SupplierShippingCountries { get; set; } = new List<Supplier>();
}
