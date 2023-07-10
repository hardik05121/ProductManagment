
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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
        public void Update(Company obj)
        {
            var objFromDb = _db.Companies.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Currency = obj.Currency;
                objFromDb.Address = obj.Address;
                objFromDb.PhoneNumber = obj.PhoneNumber;
                objFromDb.Email = obj.Email;
                objFromDb.IsActive = obj.IsActive;

                if (obj.CompanyImage != null)
                {
                    objFromDb.CompanyImage = obj.CompanyImage;
                }
            }
        }
    }
}
