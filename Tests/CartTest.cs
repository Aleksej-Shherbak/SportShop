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

        [Test]
        public void Can_App_Quantity_For_Existing_Line()
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
            
            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p2, 10);

            var result = cart.Lines.ToArray();
            
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 1);
            Assert.AreEqual(result[1].Quantity, 11);

        }

        [Test]
        public void Can_Remove_Item()
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
            
            Product p3 = new Product
            {
                Id = 3, 
                Name = "P3"
            };
            
            Cart cart = new Cart();
            
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);
            
            cart.RemoveLine(p2);
            cart.RemoveLine(p1);

            
            Assert.AreEqual(cart.Lines.FirstOrDefault(c => c.Product == p2)?.Quantity, 3);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [Test]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product
            {
                Id = 1, 
                Name = "P1",
                Price = 100
            };
            
            Product p2 = new Product
            {
                Id = 2, 
                Name = "P2",
                Price = 100
            };
            
            Cart cart = new Cart();
            
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            decimal result = cart.ComputeTotalValue();
            
            Assert.AreEqual(result, 500);
        }

        [Test]
        public void Can_Clean_Cart()
        {
            Product p1 = new Product
            {
                Id = 1, 
                Name = "P1",
                Price = 100
            };
            
            Product p2 = new Product
            {
                Id = 2, 
                Name = "P2",
                Price = 100
            };
            
            Cart cart = new Cart();
            
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            
            cart.Clear();
            
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
    }
}