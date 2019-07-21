using System;
using System.Linq;
using System.Threading.Tasks;
using Domains.Abstract;
using Domains.Concrete;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;
        
        
        public AdminController(IProductRepository repository)
        {
            _repository = repository;
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
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                
                TempData["message"] = $"{product.Name} has been saved";
                
                return RedirectToAction("Index","Admin");
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

            return RedirectToAction("Index");
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }
    }
}