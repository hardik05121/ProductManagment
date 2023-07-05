using System.ComponentModel.DataAnnotations;

namespace ProductManagment_Models.Models
{
    [MetadataType(typeof(BrandMetadataPartial))]
    public partial class Brand
    {
    }

    public class BrandMetadataPartial
    {
        [StringLength(50)]
        [Display(Name = "Brand Name*")]
        [MaxLength(10, ErrorMessage = "BrandName must be 10 characters or less")]
        public string BrandName { get; set; }

        [StringLength(450)]
        public string BrandImage { get; set; }
    }
}
