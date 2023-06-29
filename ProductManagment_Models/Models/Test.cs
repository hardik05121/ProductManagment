using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[Table("Test")]
public partial class Test
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string? Name { get; set; }

    public bool IsActive { get; set; }
}
