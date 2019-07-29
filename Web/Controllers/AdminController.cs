using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdminController(IProductRepository repository, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            return View(_repository.Products.OrderByDescending(p => p.Id));
        }

        public ViewResult Edit(int id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == id);

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string extension = Path.GetExtension(image.FileName);

                    var fileName = Guid.NewGuid() + extension;

                    var path = "/Files/" + fileName;

                    if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/Files/"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/Files/");
                    }

                    Product oldProduct = _repository.Products.FirstOrDefault(p => p.Id == product.Id);

                    // delete old image
                    if (oldProduct != null && 
                        System.IO.File.Exists(_hostingEnvironment.WebRootPath + oldProduct.Image))
                    {
                        System.IO.File.Delete(_hostingEnvironment.WebRootPath + oldProduct.Image);
                    }

                    using (var fileStream = new FileStream(_hostingEnvironment.WebRootPath + path,
                        FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    product.Image = path;
                }

                _repository.SaveProduct(product);

                TempData["message"] = $"{product.Name} has been saved";

                return RedirectToAction("Index", "Admin");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Product deletedProduct = await _repository.DeleteProductAsync(id);

            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }

            // delete image 
            if (!string.IsNullOrEmpty(deletedProduct?.Image) &&
                System.IO.File.Exists(_hostingEnvironment.WebRootPath + deletedProduct.Image))
            {
                System.IO.File.Delete(_hostingEnvironment.WebRootPath + deletedProduct.Image);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }
    }
}