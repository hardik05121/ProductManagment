using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_03_07.Data;
using Practice_03_07.Models;
using Practice_03_07.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDb _db;
        private readonly IWebHostEnvironment _webHostEnivornment;

        public ProductController(AppDb db, IWebHostEnvironment webHostEnivornment)
        {
            _db = db;
            _webHostEnivornment = webHostEnivornment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Products.Include(u => u.Category);
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Categories.Select(e => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                })
            };
            if(id == null)
            {
                //create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Products.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnivornment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //creating
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extesnion = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + extesnion), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    productVM.Product.Image = filename + extesnion;

                    _db.Products.Add(productVM.Product);

                }
                else
                {
                    var objFromdb = _db.Products.AsNoTracking().FirstOrDefault(e => e.Id == productVM.Product.Id);

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extenstion = Path.GetExtension(files[0].FileName);

                        var oldfile = Path.Combine(upload, objFromdb.Image);

                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                        using (var filestream = new FileStream(Path.Combine(upload, filename + extenstion), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);

                        }

                        productVM.Product.Image = filename + extenstion;

                    }
                    else
                    {
                        productVM.Product.Image = objFromdb.Image;
                    }
                    _db.Products.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Categories.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
        }


        [HttpGet]

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _db.Products.Include(e => e.Category).FirstOrDefault(u => u.Id == id);

            if(product== null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnivornment.WebRootPath + WC.ImagePath;
            var oldfile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }

            _db.Products.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
