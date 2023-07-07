using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SqlSubCategoryRepository : ISubcategory
    {
        AppDbContext _subcategories;
        public SqlSubCategoryRepository(AppDbContext subcategories)
        {
            _subcategories = subcategories;
        }

        public bool AddSubcategory(Subcategory subcategory)
        {
            if (CheckInsertUnique(subcategory.Name))
            {
                
                _subcategories.Add(subcategory);
                _subcategories.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckInsertUnique(string name)
        {
            Subcategory isDuplicate = _subcategories.Subcategories.FirstOrDefault(each => each.Name.ToLower() == name.ToLower());
            return isDuplicate == null ? true : false;
        }

        public bool CheckUpdateUnique(string name)
        {
            Subcategory subcategory = _subcategories.Subcategories.FirstOrDefault(each => each.Name.ToLower() == name.ToLower());
            if (subcategory == null)
                return true;
            return false;
        }

        public bool Delete(int id)
        {
            Subcategory subcategory = _subcategories.Subcategories.FirstOrDefault(each => each.Sid == id);
            if(subcategory != null)
            {
                _subcategories.Subcategories.Remove(subcategory);
                _subcategories.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Subcategory> GetAllSubcategories()
        {
            return _subcategories.Subcategories;
        }

        public Subcategory GetSubcategoryById(int id)
        {
            return _subcategories.Subcategories.FirstOrDefault(e => e.Sid == id);
        }

        public bool UpdateSubCategory(Subcategory subcategory)
        {
            if (CheckUpdateUnique(subcategory.Name))
            {
                _subcategories.ChangeTracker.Clear();
                _subcategories.Subcategories.Update(subcategory);
                _subcategories.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
