using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_Models.Models
{
    [ModelMetadataType(typeof(CityMetadata))]
    public partial class City
    {

    }

    public partial class CityMetadata
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string CityName { get; set; } = null!;

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("Cities")]
        [ValidateNever]
        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();


        public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

        [ForeignKey("StateId")]
        [InverseProperty("Cities")]
        [ValidateNever]
        public virtual State State { get; set; } = null!;

        public virtual ICollection<Supplier> SupplierBillingCities { get; set; } = new List<Supplier>();


        public virtual ICollection<Supplier> SupplierCities { get; set; } = new List<Supplier>();

        public virtual ICollection<Supplier> SupplierShippingCities { get; set; } = new List<Supplier>();
    }
}
