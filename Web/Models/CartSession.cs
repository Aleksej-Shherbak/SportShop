using System;
using Domains.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Web.Infrastructure;

namespace Web.Models
{
    public class CartSession : Cart
    {
        [JsonIgnore] 
        public ISession Session { get; set; }
        
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            CartSession cart = session?.GetObject<CartSession>("cart")
                               ?? new CartSession();
            cart.Session = session;
            return cart;
        }
        
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetObject("cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetObject("cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("cart");
        }
    }
}