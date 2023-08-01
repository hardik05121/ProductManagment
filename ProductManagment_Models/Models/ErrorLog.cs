using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class ErrorLog
{
    [Key]
    public int ErrorId { get; set; }

    [StringLength(50)]
    public string? ErrorMessage { get; set; }

    [StringLength(50)]
    public string? StackTrace { get; set; }

    public DateTime? ErrorDate { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
