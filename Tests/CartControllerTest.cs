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

            CartController controller = new CartController(mock.Object, cart, null);

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
                new Product {Id = 1, Name = "P1", Category = "Appless"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, cart, null);

            RedirectToActionResult result = controller.AddToCart(2);

            Assert.AreEqual(result.ActionName, "Index");
        }

        [Test]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();

            CartController controller = new CartController(null, cart, null);

            CartIndexViewModel result = (CartIndexViewModel) controller.Index().ViewData.Model;

            Assert.AreSame(result.Cart, cart);
        }

        [Test]
        public void Cannot_Checkout_Empty_Cart()
        {
            var mock = new Mock<IOrderProcessor>();

            var cart = new Cart();

            var shippingDetails = new ShippingDetails();

            var controller = new CartController(null, cart, mock.Object);

            ViewResult result = controller.Checkout(shippingDetails);

            // Проверка, что заказ не был передан на обработку
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), 
                It.IsAny<ShippingDetails>()), Times.Never);
            
            // Проверка, что метод вернул стандартное представление
            Assert.AreEqual(null, result.ViewName);
            
            // Проверка, что представлению была передана невалидная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Cannot_Checkout_Invalid_Shipping_Data()
        {
            var mock = new Mock<IOrderProcessor>();

            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            
            var controller = new CartController(null, cart, mock.Object);
            
            controller.ModelState.AddModelError("error", "error");
            
            // Попытка перехода к оплате
            ViewResult res = controller.Checkout(new ShippingDetails());
            
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), 
                    It.IsAny<ShippingDetails>()), Times.Never);
            
            // Метод возвращает стандартное (без имени) представление
            Assert.AreEqual(null, res.ViewName);

            // Представлению передается невалидная модель
            Assert.AreEqual(false, res.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Can_Checkout_And_Submit_Order()
        {
            var mock = new Mock<IOrderProcessor>();
            
            var cart = new Cart();
            
            cart.AddItem(new Product{ Id = 1, Name = "Product 1", Category = "Apple"}, 1);
            
            var controller = new CartController(null, cart, mock.Object);

            ViewResult result = controller.Checkout(new ShippingDetails());
            
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), 
                It.IsAny<ShippingDetails>()), Times.Once);

            // Проверка что представление "Completed"
            Assert.AreEqual("Completed", result.ViewName);
            
            // Представление получает валидную модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}