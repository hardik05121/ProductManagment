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

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public long MobileNumber { get; set; }

    [StringLength(450)]
    public string? Email { get; set; }

    [StringLength(450)]
    public string? Address { get; set; }

    [StringLength(450)]
    public string? UserImage { get; set; }
    public DateTime? CreatedDate { get; set; }
    [NotMapped]
    public string RoleId { get; set; }
    [NotMapped]
    public string Role { get; set; }
    [NotMapped]
    public IEnumerable<SelectListItem> RoleList { get; set; }

}
