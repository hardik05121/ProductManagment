using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
//using ProductManagment_Models.ModelsMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(Brand obj)
        {
            var objFromDb = _db.Brands.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.BrandName = obj.BrandName;

                if (obj.BrandImage != null)
                {
                    objFromDb.BrandImage = obj.BrandImage;
                }
            }
            // _db.Brands.Update(obj);
        }
    }
}
