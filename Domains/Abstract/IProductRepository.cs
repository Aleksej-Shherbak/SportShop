using System.Collections.Generic;
using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        void SaveProduct(Product product);

        Task<Product> DeleteProductAsync(int id);
    }
}