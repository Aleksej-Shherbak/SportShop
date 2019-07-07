using System;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Web.Infrastructure.Binders
{
    public class CartModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Cart))
            {
                return new BinderTypeModelBinder(typeof(CartModelBinder));
            }

            return null;
        }
    }
}