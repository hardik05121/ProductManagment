
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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Code = obj.Code;
                objFromDb.BrandId = obj.BrandId;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.UnitId = obj.UnitId;
                objFromDb.WarehouseId = obj.WarehouseId;
                objFromDb.TaxId = obj.TaxId;
                objFromDb.SkuCode = obj.SkuCode;
                objFromDb.SkuName = obj.SkuName;
                objFromDb.SalesPrice = obj.SalesPrice;
                objFromDb.PurchasePrice = obj.PurchasePrice;
                objFromDb.Mrp = obj.Mrp;
                objFromDb.BarcodeNumber = obj.BarcodeNumber;
                objFromDb.Description = obj.Description;
                objFromDb.IsActive = obj.IsActive;
                objFromDb.UpdatedDate = DateTime.Now;


                if (obj.ProductImage != null)
                {
                    objFromDb.ProductImage = obj.ProductImage;
                }
            }
        }
    }
}
