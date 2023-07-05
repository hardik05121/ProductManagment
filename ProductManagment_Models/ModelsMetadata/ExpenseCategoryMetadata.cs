using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(ExpenseCategoryMetadata))]
public partial class ExpenseCategory
{

}

public partial class ExpenseCategoryMetadata
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ExpenseCategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
