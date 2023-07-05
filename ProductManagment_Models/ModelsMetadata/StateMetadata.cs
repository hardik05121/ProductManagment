using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(StateMetadata))]
public partial class State
{

}

public partial class StateMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Display(Name = "StateName*")]
    public string StateName { get; set; } = null!;

    [Display(Name = "Select YourCountry")]
    public int CountryId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("CountryId")]
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public virtual ICollection<Supplier> SupplierBillingStates { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierShippingStates { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierStates { get; set; } = new List<Supplier>();
}



