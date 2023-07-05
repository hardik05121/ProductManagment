using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(UserMetadata))]
public partial class User
{

}

public partial class UserMetadata : IdentityUser
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
