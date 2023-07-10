using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SQLProductRepository:IProduct
    {
        AppDbContext _products;

        public SQLProductRepository(AppDbContext products)
        {
            _products = products;
        }
public bool AddProduct(Product p)
        {
            if (CheckInsertUnique(p.Name, p.CatId))
            {
                _products.Add(p);
                _products.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckInsertUnique(string name, int catId)
        {
           
            Product isDuplicate = _products.Products.FirstOrDefault(each => each.CatId == catId && name.ToLower() == each.Name.ToLower());
            return isDuplicate == null ? true : false;
        }

        public bool UpdateProduct(Product p)
        {
            if (CheckUpdateUnique(p.Name, p.CatId, p.Id))
            {
                _products.ChangeTracker.Clear();
                _products.Products.Update(p);
                _products.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckUpdateUnique(string name, int catId, int prodId)
        {
            Product prod = _products.Products.FirstOrDefault(each => each.Name.ToLower() == name.ToLower());
            if (prod == null)
                return true;
            prod = _products.Products.FirstOrDefault(each => each.Name.ToLower() == name.ToLower() && each.CatId == catId && each.Id == prodId);
            if (prod != null)
                return true;
            bool flag = false;

            foreach(Product p in _products.Products)
            {
                if(p.Name.ToLower() == name.ToLower() && p.CatId != catId)
                {
                    flag = true;
                }
                else if(p.Name.ToLower()==name.ToLower() && p.CatId == catId)
                {
                    flag = false;
                }
            }
                return flag;   
        }

        public bool Delete(int id)
        {
            Product p = _products.Products.FirstOrDefault(Each => Each.Id == id);
            if(p != null)
            {
                _products.Remove(p);
                _products.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _products.Products;
        }

        public Product GetProductById(int id)
        {
            return _products.Products.FirstOrDefault(each => each.Id == id);
        }
        public Product GetProductByCatId(int id)
        {
            return _products.Products.FirstOrDefault(each => each.CatId == id);
        }


    }
}
