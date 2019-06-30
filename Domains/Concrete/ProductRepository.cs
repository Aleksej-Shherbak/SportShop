using System.Collections.Generic;
using Domains.Abstract;
using Domains.Entities;


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
    }
}