using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Web.Controllers;
using Web.Models;

namespace Tests
{
    public class CartControllerTest
    {
        [Test]
        public void Can_Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"}
            });
            
            Cart cart = new Cart();
            
            CartController controller = new CartController(mock.Object, cart);

            controller.AddToCart(1);
            
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.Id, 1);
        }
        
        [Test]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ Id = 1, Name = "P1", Category = "Appless"}
            }.AsQueryable());
            
            Cart cart = new Cart();
            
            CartController controller = new CartController(mock.Object, cart);

            RedirectToActionResult result = controller.AddToCart(2);
            
            Assert.AreEqual(result.ActionName, "Index");
        }

        [Test]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            
            CartController controller = new CartController(null, cart);

            CartIndexViewModel result = (CartIndexViewModel) controller.Index().ViewData.Model;
            
            Assert.AreSame(result.Cart, cart);
        }
    }
}