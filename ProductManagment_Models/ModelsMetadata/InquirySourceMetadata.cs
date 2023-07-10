using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(InquirySourceMetadata))]
public partial class InquirySource
{

}

public partial class InquirySourceMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string InquirySourceName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
