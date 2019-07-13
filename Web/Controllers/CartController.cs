using System;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly Cart _cart;

        private readonly IOrderProcessor _orderProcessor;


        public CartController(IProductRepository repository, Cart cart, IOrderProcessor orderProcessor)
        {
            _repository = repository;
            _cart = cart;
            _orderProcessor = orderProcessor;
        }

        public RedirectToActionResult AddToCart(int id)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCart(int id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Index()
        {
            var res = new CartIndexViewModel
            {
                Cart = _cart
            };
            return View(res);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(ShippingDetails shippingDetails)
        {
            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(_cart, shippingDetails);

                _cart.Clear();

                return View("Completed");
            }

            return View(shippingDetails);
        }
    }
}