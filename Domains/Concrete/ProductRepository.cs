using System;
using System.Collections.Generic;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace Domains.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _applicationContext;

        public ProductRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEnumerable<Product> Products
        {
            get { return _applicationContext.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                _applicationContext.Add(product);
            }
            else
            {
                Product dbEntity = _applicationContext.Products.Find(product.Id);

                if (dbEntity != null)
                {
                    dbEntity.Name = product.Name;
                    dbEntity.Description = product.Description;
                    dbEntity.Price = product.Price;
                    dbEntity.Category = product.Category;
                }
            }

            _applicationContext.SaveChanges();
        }
        
    }
}