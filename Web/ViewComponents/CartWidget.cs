using Domains.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    public class CartWidget : ViewComponent
    {
        private readonly Cart _cart;
        
        public CartWidget(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}