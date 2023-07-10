using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategory _categoryRepository;
        private IProduct _productRepository;


        public CategoryController(ICategory categoryRepository, IProduct productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }



        [HttpGet]

        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.GetAllCategories();
        }



        [HttpGet]
        [Route("getcategory")]

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);
        }



        [HttpPost]
        [Route("createcategory")]
        public bool AddCategory([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepository.CheckInsertUnique(category.Name))
                {
                    return _categoryRepository.AddCategory(category);
                }
            }
            return false;
        }



        [HttpPut]
        [Route("updatecategory")]

        public bool UpdateCategory([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepository.CheckUpdateUnique(category.Name))
                {
                    return _categoryRepository.UpdateCategory(category);
                }
            }
            return false;
        }



        [HttpDelete]
        [Route("deletecategory")]
        public bool DeleteCategoryById(int id)
        {
            var products = _productRepository.GetProductByCatId(id);
                
         if (products == null){ 
            return _categoryRepository.Delete(id);
         }

            return false;
        }
    }
}