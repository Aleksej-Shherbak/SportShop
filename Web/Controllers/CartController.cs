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

        public RedirectToActionResult AddToCart(int id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCart(int id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            
            return RedirectToAction("Index");
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("cart");

            if (cart == null)
            {
                cart = new Cart();
                
                HttpContext.Session.SetObject("cart", cart);
            }

            return cart;
        }

        private Cart SaveCart(Cart cart)
        {
            if (cart == null)
            {
                cart = HttpContext.Session.GetObject<Cart>("cart");
            }

            HttpContext.Session.SetObject("cart", cart);
            
            return HttpContext.Session.GetObject<Cart>("cart");
        }
        
        public ViewResult Index()
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = Url.RouteUrl("Home")
            });
        }
    }
}