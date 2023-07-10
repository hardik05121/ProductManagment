using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private ISubcategory _subcategoryRepository;
        private IProduct _product;

        public SubCategoryController(ISubcategory subcategorRepository,IProduct product)
        {
            _subcategoryRepository = subcategorRepository; 
            _product = product;
        }

        [HttpGet]

        public IEnumerable<Subcategory> GetSubcategories()
        {
            return _subcategoryRepository.GetAllSubcategories();
        }
        

        [HttpGet]
        [Route("getsubcategory")]

        public Subcategory GetSubcategoryById(int id)
        {
            return _subcategoryRepository.GetSubcategoryById(id);
        }

        [HttpPost]
        [Route("createsubcategory")]

        public bool AddSubcategory([FromForm] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                if (_subcategoryRepository.CheckInsertUnique(subcategory.Name))
                {
                    return _subcategoryRepository.AddSubcategory(subcategory);
                }
            }
            return false;
        }

        [HttpPut]
        [Route("updatesubcategory")]
        public bool UpdateSUbCategory([FromForm] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                if (_subcategoryRepository.CheckUpdateUnique(subcategory.Name))
                {
                    return _subcategoryRepository.UpdateSubCategory(subcategory);
                }
            }
            return false;
        }

        [HttpDelete]
        [Route("deletesubcategory")]

        public bool DeleteSubCategory(int id)
        {
            var products = _product.GetProductByCatId(id);

            if (products == null)
            {
                return _subcategoryRepository.Delete(id);
            }
            return false;
        }



    }
}
