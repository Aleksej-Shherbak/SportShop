using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;

        public CartController(IProductRepository repository)
        {
            _repository = repository;
        }

        public RedirectToActionResult AddToCart(Cart cart, int id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                cart.AddItem(product, 1);
                HttpContext.Session.SetObject("cart", cart);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCart(Cart cart, int id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                cart.RemoveLine(product);
                HttpContext.Session.SetObject("cart", cart);
            }

            return RedirectToAction("Index");
        }
        
        public ViewResult Index(Cart cart)
        {
            var res = new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = Url.RouteUrl("Home")
            };
            return View(res);
        }
    }
}