using System.Collections.Generic;
using System.Linq;

namespace Domains.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(p => p.Product.Id == product.Id);

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            var productLine = lineCollection.FirstOrDefault(l => l.Product.Id == product.Id);

            if (productLine != null)
            {
                if (productLine.Quantity > 1)
                {
                    productLine.Quantity = productLine.Quantity - 1;
                }
                else
                {
                    lineCollection.RemoveAll(l => l.Product.Id == product.Id);
                }
            }
        }

        public virtual void Clear()
        {
            lineCollection.Clear();
        }

        public virtual decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        public virtual IEnumerable<CartLine> Lines => lineCollection;
        
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}