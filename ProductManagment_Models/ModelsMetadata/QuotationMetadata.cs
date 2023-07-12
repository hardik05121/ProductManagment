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
     [ModelMetadataType(typeof(QuotationMetadata))]
    public partial class Quotation
    {

    }

    public partial class QuotationMetadata
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Select SupplierName")]
        public int SupplierId { get; set; }

        [StringLength(450)]
        [Display(Name = "Select UserName")]
        public string UserId { get; set; } = null!;

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

        public virtual ICollection<PurChaseOrder> PurChaseOrders { get; set; } = new List<PurChaseOrder>();
        public virtual ICollection<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();

        [ForeignKey("SupplierId")]
        [ValidateNever]
        public virtual Supplier Supplier { get; set; } = null!;

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual AspNetUser User { get; set; } = null!;

    }
}