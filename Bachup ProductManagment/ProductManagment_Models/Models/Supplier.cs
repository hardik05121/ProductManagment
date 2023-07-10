using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Supplier
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string? ContactPerson { get; set; }

    [StringLength(450)]
    public string? Email { get; set; }

    [StringLength(450)]
    public string? WebSite { get; set; }

    public long MobileNumber { get; set; }

    public long? PhoneNumber { get; set; }

    [StringLength(450)]
    public string? Address { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    [StringLength(450)]
    public string BillingAddress { get; set; } = null!;

    public int BillingCountryId { get; set; }

    public int BillingStateId { get; set; }

    public int BillingCityId { get; set; }

    [StringLength(450)]
    public string ShippingAddress { get; set; } = null!;

    public int ShippingCountryId { get; set; }

    public int ShippingStateId { get; set; }

    public int ShippingCityId { get; set; }

    [StringLength(450)]
    public string? Description { get; set; }

    [StringLength(450)]
    public string? SupplierImage { get; set; }

    [ForeignKey("BillingCityId")]
    [InverseProperty("SupplierBillingCities")]
    public virtual City BillingCity { get; set; } = null!;

    [ForeignKey("BillingCountryId")]
    [InverseProperty("SupplierBillingCountries")]
    public virtual Country BillingCountry { get; set; } = null!;

    [ForeignKey("BillingStateId")]
    [InverseProperty("SupplierBillingStates")]
    public virtual State BillingState { get; set; } = null!;

    [ForeignKey("CityId")]
    [InverseProperty("SupplierCities")]
    public virtual City City { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("SupplierCountries")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<PurChaseOrder> PurChaseOrders { get; set; } = new List<PurChaseOrder>();

    [InverseProperty("Supplier")]
    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    [ForeignKey("ShippingCityId")]
    [InverseProperty("SupplierShippingCities")]
    public virtual City ShippingCity { get; set; } = null!;

    [ForeignKey("ShippingCountryId")]
    [InverseProperty("SupplierShippingCountries")]
    public virtual Country ShippingCountry { get; set; } = null!;

    [ForeignKey("ShippingStateId")]
    [InverseProperty("SupplierShippingStates")]
    public virtual State ShippingState { get; set; } = null!;

    [ForeignKey("StateId")]
    [InverseProperty("SupplierStates")]
    public virtual State State { get; set; } = null!;
}
