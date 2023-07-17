using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Quotation
{
    [Key]
    public int Id { get; set; }

    public int SupplierId { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(450)]
    public string QuotationNumber { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    [StringLength(450)]
    public string? TermCondition { get; set; }

    [StringLength(450)]
    public string? Notes { get; set; }

    [StringLength(450)]
    public string? ScanBarCode { get; set; }

    public double? GrandTotal { get; set; }

    [InverseProperty("Quotation")]
    public virtual ICollection<PurChaseOrder> PurChaseOrders { get; set; } = new List<PurChaseOrder>();

    [InverseProperty("Quotation")]
    public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Quotations")]
    public virtual Supplier Supplier { get; set; } = null!;
}
