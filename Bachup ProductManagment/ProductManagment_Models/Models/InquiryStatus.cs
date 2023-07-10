using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class InquiryStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string InquiryStatusName { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("InquiryStatus")]
    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
}
