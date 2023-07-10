
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository
{
    public class QuotationXproductRepository : Repository<QuotationXproduct>, IQuotationXproductRepository
    {
        private ApplicationDbContext _db;
        public QuotationXproductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(QuotationXproduct obj)
        {
            _db.QuotationXproducts.Update(obj);
        }
    }
}

