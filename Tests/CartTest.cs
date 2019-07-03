using System.Linq;
using Domains.Entities;
using NUnit.Framework;

namespace Tests
{
    public class CartTest
    {
        [Test]
        public void Can_Add_New_Line()
        {
            Product p1 = new Product
            {
                Id = 1, 
                Name = "P1"
            };
            
            Product p2 = new Product
            {
                Id = 2, 
                Name = "P2"
            };
            
            var cart = new Cart();
            
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            var cartLines = cart.Lines.ToArray();
            
            Assert.AreEqual(cartLines.Length, 2);
            Assert.AreEqual(cartLines[0].Product, p1);
            Assert.AreEqual(cartLines[1].Product, p2);
        }
    }
}