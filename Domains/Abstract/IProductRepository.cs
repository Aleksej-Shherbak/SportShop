using System.Collections.Generic;
using Domains.Entities;

namespace Domains.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}