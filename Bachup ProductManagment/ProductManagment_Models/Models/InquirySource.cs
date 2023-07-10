using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class InquirySource
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string InquirySourceName { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("InquirySource")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
