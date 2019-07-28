using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                product.CreatedAt = DateTime.UtcNow;
                _applicationContext.Products.Add(product);
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
                    dbEntity.Image = product.Image;
                }
            }

            _applicationContext.SaveChanges();
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            Product product = await _applicationContext.Products.FindAsync(id);

            if (product != null)
            {
                _applicationContext.Products.Remove(product);
                _applicationContext.SaveChanges();
            }

            return product;
        }
    }
}