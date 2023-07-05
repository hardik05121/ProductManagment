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
    public class ExpenseRepository : Repository<ExpenseMetadata>, IExpenseRepository
    {
        private ApplicationDbContext _db;
        public ExpenseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(ExpenseMetadata obj)
        {
            _db.Expenses.Update(obj);
        }
    }
}
