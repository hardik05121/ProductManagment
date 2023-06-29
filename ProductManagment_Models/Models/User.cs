using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class User
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

    [StringLength(50)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string ConfirmPassword { get; set; } = null!;

    public int RoleId { get; set; }

    [StringLength(450)]
    public string? Address { get; set; }

    [StringLength(450)]
    public string? UserImage { get; set; }

    public DateTime? CreatedDate { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
