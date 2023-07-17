using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[Table("PurChaseOrder")]
public partial class PurChaseOrder
{
    [Key]
    public int Id { get; set; }

    public int QuotationId { get; set; }

    [Column("PONumber")]
    [StringLength(50)]
    public string Ponumber { get; set; } = null!;

    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [StringLength(50)]
    public string? IsReturn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DeliveryDate { get; set; }

    public int SupplierId { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [StringLength(50)]
    public string? TermCondition { get; set; }

    [StringLength(450)]
    public string? Notes { get; set; }

    [StringLength(450)]
    public string? ScanBarcode { get; set; }

    public double? GrandTotal { get; set; }

    [InverseProperty("PurChaseOrder")]
    public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

    [ForeignKey("QuotationId")]
    [InverseProperty("PurChaseOrders")]
    public virtual Quotation Quotation { get; set; } = null!;

    [ForeignKey("SupplierId")]
    [InverseProperty("PurChaseOrders")]
    public virtual Supplier Supplier { get; set; } = null!;
}
