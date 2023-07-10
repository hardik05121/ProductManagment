using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public interface ICategory
    {
        public Category GetCategoryById(int id);
        public IEnumerable<Category> GetAllCategories();
        public bool AddCategory(Category category);
        public bool UpdateCategory(Category category);

        public bool Delete(int id);
        public bool CheckUpdateUnique(string name);
        public bool CheckInsertUnique(string name);
    }
}
