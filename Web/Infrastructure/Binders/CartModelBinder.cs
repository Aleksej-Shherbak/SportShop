using System;
using System.Threading.Tasks;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        public const string SessionKey = "cart";
        
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            
            Cart cart = null;

            if (bindingContext.HttpContext.Session != null)
            {
                cart = bindingContext.HttpContext.Session.GetObject<Cart>(SessionKey);
            }

            if (cart == null)
            {
                cart = new Cart();

                if (bindingContext.HttpContext.Session != null)
                {
                    bindingContext.HttpContext.Session.SetObject(SessionKey, cart);

                    bindingContext.Result = ModelBindingResult.Success(cart);
                }
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(cart);
            }

            return Task.CompletedTask;
        }
    }
}