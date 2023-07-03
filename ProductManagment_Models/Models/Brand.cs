using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Brand
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [MaxLength(10, ErrorMessage = "BrandName must be 10 characters or less")]
    public string BrandName { get; set; } = null!;

    [StringLength(450)]
    public string? BrandImage { get; set; }

    [InverseProperty("Brand")]

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    //[Display(Name = "Price for 50+")]
    //[Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
    //[StringLength(60, MinimumLength = 3)]
    //[DataType(DataType.Date)]
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
    //[Range(22, 60)]
    //ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">  
    //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
    //[DataType(DataType.PhoneNumber)]
    //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
    //[RegularExpression(@"^(0|91)?[6-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]
    //[RegularExpression(@"^\\(?(\[0-9\]{3})\\)?\[-.●\]?(\[0-9\]{3})\[-.●\]?(\[0-9\]{4})$", ErrorMessage = "The PhoneNumber field is not a valid phone number")\]
    // public string PhoneNumber { get; set; }
    //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
    //public string PhoneNumber
    //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
    //public string Mobile { get; set; }
    //    [StringLength(13, MinimumLength = 10)]
    //    @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"
    //    ^([\+]?33[-]?|[0])?[1-9][0-9]{8}$
    //@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$"
    //    public string MobileNo { get; set; }
    // /^(([^<>()[\]\\.,;:\s@@\"]+(\.[^<>()[\]\\.,;:\s@@\"]+)*)|(\".+\"))@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
}
