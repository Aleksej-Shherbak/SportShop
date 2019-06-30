using System.Collections.Generic;
using Domains.Entities;

namespace Web.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginInfo PaginInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}   