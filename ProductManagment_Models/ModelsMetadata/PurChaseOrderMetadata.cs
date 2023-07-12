using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_Models.Models
{
   [ModelMetadataType(typeof(PurChaseOrderMetadata))]
    public partial class PurChaseOrder
    {

    }

    public partial class PurChaseOrderMetadata
    {
        [Key]
        public int Id { get; set; }

        public int QuotationId { get; set; }

        [Column("PONumber")]
        [StringLength(50)]
        [Display(Name = "PO Number*")]
        public string Ponumber { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "Your PaymentStatus")]
        public string? PaymentStatus { get; set; }

        [StringLength(50)]
        [Display(Name = "Is Return?")]
        public string? IsReturn { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "Orderdate*")]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "DeliveryDate*")]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Select Supplier*")]
        public int SupplierId { get; set; }

        [StringLength(450)]
        [Display(Name = "Select User*")]
        public string UserId { get; set; } = null!;

        [StringLength(50)]
        public string? TermCondition { get; set; }

        [StringLength(450)]
        public string? Notes { get; set; }

        [StringLength(450)]
        public string? ScanBarcode { get; set; }

        public double? GrandTotal { get; set; }

        public virtual ICollection<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; } = new List<PurChaseOrderXproduct>();

        [ForeignKey("QuotationId")]
        [ValidateNever]
        public virtual Quotation Quotation { get; set; } = null!;

        [ForeignKey("SupplierId")]
        [ValidateNever]
        public virtual Supplier Supplier { get; set; } = null!;

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual AspNetUser User { get; set; } = null!;
    }
}