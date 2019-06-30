using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Models;


namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        public int PageSize = 4;
        
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var productsList = new ProductsListViewModel
            {
                Products = _repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.CreatedAt)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PaginInfo = new PaginInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Products.Count(p => category == null || p.Category == category)
                },
                CurrentCategory = category
            };
            
            return View(productsList);
        }
    }
}