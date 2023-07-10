using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public interface ISubcategory
    {
        public Subcategory GetSubcategoryById(int id);

        public IEnumerable<Subcategory> GetAllSubcategories();

        public bool AddSubcategory(Subcategory subcategory);

        public bool UpdateSubCategory(Subcategory subcategory);

        public bool Delete(int id);
        public bool CheckUpdateUnique(string name);
        public bool CheckInsertUnique(string name);

    }
}
