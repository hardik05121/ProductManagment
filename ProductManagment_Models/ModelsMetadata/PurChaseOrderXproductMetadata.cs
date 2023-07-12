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
      [ModelMetadataType(typeof(PurChaseOrderXproductMetadata))]
    public partial class PurChaseOrderXproduct
    {

    }

    public partial class PurChaseOrderXproductMetadata
    {
        [Key]
        public int Id { get; set; }

        public int PurChaseOrderId { get; set; }


        [Display(Name = "Select Warehouse*")]
        public int WarehouseId { get; set; }


        [Display(Name = "Select Product*")]
        public int ProductId { get; set; }


        [Display(Name = "Select BaseUnit*")]
        public int UnitId { get; set; }


        [Display(Name = "Select TaxPersentage*")]
        public int TaxId { get; set; }


        [Display(Name = "Product Price*")]
        public double? Price { get; set; }


        [Range(1, 10, ErrorMessage = "Please enter a value between 1 and 10")]
        public int? Quantity { get; set; }

        public double? Subtotal { get; set; }

        public double? Discount { get; set; }


        [ForeignKey("ProductId")]
        [ValidateNever]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("PurChaseOrderId")]
        [ValidateNever]
        public virtual PurChaseOrder PurChaseOrder { get; set; } = null!;

        [ForeignKey("TaxId")]
        [ValidateNever]
        public virtual Tax Tax { get; set; } = null!;

        [ForeignKey("UnitId")]
        [ValidateNever]
        public virtual Unit Unit { get; set; } = null!;

        [ForeignKey("WarehouseId")]
        [ValidateNever]
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}
