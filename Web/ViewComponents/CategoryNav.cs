using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domains.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Web.ViewComponents
{
    public class CategoryNav : ViewComponent
    {
        private readonly IProductRepository _repository;

        public CategoryNav(IProductRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke(string currentCategory)
        {
            ViewBag.SelectedCategory = currentCategory;
            
            var categories = _repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);
            
            return View(categories);
        }
    }
}