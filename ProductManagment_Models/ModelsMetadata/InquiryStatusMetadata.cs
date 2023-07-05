using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(InquiryStatusMetadata))]
public partial class InquiryStatus
{

}

public partial class InquiryStatusMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string InquiryStatusName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
