using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository
{
    public class ExpenseCategoryRepository : Repository<ExpenseCategory>, IExpenseCategoryRepository
    {
        private ApplicationDbContext _db;
        public ExpenseCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(ExpenseCategory obj)
        {
            _db.ExpenseCategories.Update(obj);
        }
    }
}
