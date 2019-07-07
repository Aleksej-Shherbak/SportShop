using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Moq;
using NUnit.Framework;
using Web.Controllers;

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
    }
}