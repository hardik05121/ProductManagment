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

    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "SupplierName*")]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "PhoneNumber*")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
    public string? ContactPerson { get; set; }

    [StringLength(450)]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    [StringLength(450)]
    [Required(ErrorMessage = "Please enter the product URL.")]
    [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
        ErrorMessage = "Please enter a valid URL.")]
    public string? WebSite { get; set; }

    public long MobileNumber { get; set; }

    public long? PhoneNumber { get; set; }

    [StringLength(450)]
    [Display(Name = "Your Address*")]
    public string? Address { get; set; }

    [Display(Name = "Select Country*")]
    public int CountryId { get; set; }

    [Display(Name = "Select State*")]
    public int StateId { get; set; }

    [Display(Name = "Select City*")]
    public int CityId { get; set; }

    [StringLength(450)]
    [Display(Name = "Enter Billing Address*")]
    public string BillingAddress { get; set; } = null!;

    public int BillingCountryId { get; set; }

    public int BillingStateId { get; set; }

    public int BillingCityId { get; set; }

    [StringLength(450)]
    [Display(Name = "Enter Shipping Address*")]
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



//worked string;-

//[Display(Name = "Price for 50+")]

//[Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]

//[StringLength(60, MinimumLength = 3)]

//[DataType(DataType.Date)]
//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]

//[DataType(DataType.PhoneNumber)]
//[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
//[StringLength(13, MinimumLength = 10)]
//[RegularExpression(@"^(0|91)?[6-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]
//[RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Invalid Mobile Number.")]

//[StringLength(50)]
//[DataType(DataType.EmailAddress)]
//[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
//public string? Email { get; set; }

//[Required(ErrorMessage = "Please enter the product URL.")]
//[RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
//    ErrorMessage = "Please enter a valid URL.")]
//public string? WebSite { get; set; }

//[Display(Name = "Joining Date")]
//[DataType(DataType.Date)]
//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
//public DateTime JoiningDate { get; set; }