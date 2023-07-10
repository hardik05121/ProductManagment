using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class User : IdentityUser
{
    [Key]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [StringLength(50,MinimumLength = 2)]
    [Display(Name = "FirstName*")]
    public string FirstName { get; set; } = null!;

    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "LastName*")]
    public string LastName { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "MobileNumber*")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number && Enter a number For 10 digit.")]
    public long MobileNumber { get; set; }

    [StringLength(450)]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    [StringLength(450)]
    public string? Address { get; set; }

    [StringLength(450)]

    public string? UserImage { get; set; }
    public DateTime? CreatedDate { get; set; } =DateTime.Now;
    [NotMapped]
    public string RoleId { get; set; }
    [NotMapped]
    public string Role { get; set; }
    [NotMapped]
    public IEnumerable<SelectListItem> RoleList { get; set; }

}

// worked string;-

//[Display(Name = "Price for 50+")]

//[Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]

//[StringLength(60, MinimumLength = 3)]

//[DataType(DataType.Date)]
//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]

//[DataType(DataType.PhoneNumber)]
//[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
//[StringLength(13, MinimumLength = 10)]
//[RegularExpression(@"^(0|91)?[6-9][0-9]{9}$", ErrorMessage = "Invalid Mobile Number.")]
//[RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Invalid Mobile Number.")]

//[StringLength(50)]
//[DataType(DataType.EmailAddress)]
//[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid Email Address")]
//public string? Email { get; set; }