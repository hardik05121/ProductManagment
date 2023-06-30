using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class Expense
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ExpenseDate { get; set; }

    [StringLength(450)]
    public string? Reference { get; set; }

    public int Amount { get; set; }

    public int ExpenseCategoryId { get; set; }

    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [StringLength(450)]
    public string? Note { get; set; }

    [StringLength(450)]
    public string? ExpenseFile { get; set; }

    [ForeignKey("ExpenseCategoryId")]
    [InverseProperty("Expenses")]
    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
}
