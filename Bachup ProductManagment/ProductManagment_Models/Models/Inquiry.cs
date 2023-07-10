using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Inquiry
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Organization { get; set; } = null!;

    [StringLength(50)]
    public string ContactPerson { get; set; } = null!;

    [StringLength(50)]
    public string? Email { get; set; }

    public long? MobileNumber { get; set; }

    public long? PhoneNumber { get; set; }

    [StringLength(450)]
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
    [InverseProperty("Inquiries")]
    public virtual City City { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("Inquiries")]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("InquirySourceId")]
    [InverseProperty("Inquiries")]
    public virtual InquirySource InquirySource { get; set; } = null!;

    [ForeignKey("InquiryStatusId")]
    [InverseProperty("Inquiries")]
    public virtual InquiryStatus InquiryStatus { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Inquiries")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("StateId")]
    [InverseProperty("Inquiries")]
    public virtual State State { get; set; } = null!;
}
