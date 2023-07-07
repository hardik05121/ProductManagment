using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProduct _productRepository;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IProduct product,IWebHostEnvironment hostEnvironment)
        {
            _productRepository = product;
            _hostingEnvironment = hostEnvironment;
        }

        [HttpGet]

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAllProduct();
        }

        [HttpGet]
        [Route("getproduct")]
        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        //[HttpPost]

        //public bool Addproduct([FromBody] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return _productRepository.AddProduct(product);
                
        //    }
        //    return false;
        //}

        [HttpPost]
        [Route("createproduct")]

        public bool Addproduct([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                if (_productRepository.CheckInsertUnique(product.Name, product.CatId))
                {
                    if(product.Image != null)
                    {
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.Image.FileName;
                        string uploadFilePath = Path.Combine(uploadFolder, uniqueFileName);
                        product.Profile = uniqueFileName;
                        product.Image.CopyTo(new FileStream(uploadFilePath, FileMode.Create));

                    }
                return _productRepository.AddProduct(product);
                }
                
            }
            return false;
        }

        [HttpPut]
        [Route("updateproduct")]
        public bool UpdateProduct([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                if (_productRepository.CheckUpdateUnique
                (product.Name, product.CatId, product.Id))
                {
                    if (product.Image != null)
                    {
                        String uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        String uniqueFileName = Guid.NewGuid().ToString() + "_" + product.Image.FileName;
                        String uploadFilePath = Path.Combine(uploadFolder, uniqueFileName);
                        product.Profile = uniqueFileName;
                        product.Image.CopyTo(new FileStream(uploadFilePath, FileMode.Create));
                    }
                    return _productRepository.UpdateProduct(product);
                }
            }
            return false;
        }

        [HttpDelete]

        [Route("deleteproduct")]
        public bool DeleteProductById(int id)
        {
            return _productRepository.Delete(id);
        }
     
    }
}

