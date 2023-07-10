using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(InquiryMetadata))]
public partial class Inquiry
{

}

public partial class InquiryMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Organization { get; set; } = null!;

    [StringLength(50)]
    public string ContactPerson { get; set; } = null!;

    [StringLength(50)]
    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
              ErrorMessage = "Entered phone format is not valid.")]
    public long? MobileNumber { get; set; }
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
              ErrorMessage = "Entered phone format is not valid.")]

    public long? PhoneNumber { get; set; }

    [StringLength(450)]
    [Required(ErrorMessage = "Please enter the product URL.")]
    [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
        ErrorMessage = "Please enter a valid URL.")]
    public string? Website { get; set; }

    [StringLength(450)]
    public string? Address { get; set; }

    [StringLength(450)]
    public string? Message { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    [StringLength(50)]
    public string UserId { get; set; } = null!;

    public int ProductId { get; set; }

    public int InquiryStatusId { get; set; }

    public int InquirySourceId { get; set; }

    [ForeignKey("CityId")]
    [ValidateNever]
    public virtual City City { get; set; } = null!;

    [ForeignKey("CountryId")]
    [ValidateNever]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("InquirySourceId")]
    [ValidateNever]
    public virtual InquirySource InquirySource { get; set; } = null!;

    [ForeignKey("InquiryStatusId")]
    [ValidateNever]
    public virtual InquiryStatus InquiryStatus { get; set; } = null!;

    [ForeignKey("ProductId")]
    [ValidateNever]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("StateId")]
    [ValidateNever]
    public virtual State State { get; set; } = null!;
}
