//using ProductManagment_Models.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProductManagment_Models.Models
//{
//    public class BrandMetadata
//    {
//        [Key]
//        public int Id { get; set; }

//        [StringLength(50)]
//        [Display(Name ="Brand Name*")]
//        [MaxLength(10, ErrorMessage = "BrandName must be 10 characters or less")]
//        public string BrandName { get; set; } = null!;


//        [StringLength(450)]
//        public string? BrandImage { get; set; }

//    }

//    [MetadataType(typeof(BrandMetadata))]
//    public partial class Brand
//    {
//    }
//}
