using System.Linq;
using Domains.Abstract;
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
            return View(_repository.Products.OrderBy(p => p.Id));
        }

        public ViewResult Edit(int id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == id);

            return View(product);
        }
        
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                ViewBag.message =  $"{product.Name} has been saved";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Delete()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Create()
        {
            throw new System.NotImplementedException();
        }
    }
}