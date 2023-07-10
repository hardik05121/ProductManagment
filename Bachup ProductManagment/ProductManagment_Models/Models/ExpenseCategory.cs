using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

public partial class ExpenseCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ExpenseCategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("ExpenseCategory")]
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
