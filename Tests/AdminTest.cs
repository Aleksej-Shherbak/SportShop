using System.Collections.Generic;
using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Web.Controllers;

namespace Tests
{
    public class AdminTest
    {
        [Test]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "p1"},
                new Product {Id = 2, Name = "p2"},
                new Product {Id = 3, Name = "p3"},
            });

            var controller = new AdminController(mock.Object);

            var products = ((IEnumerable<Product>) controller.Index().ViewData.Model).ToArray();

            Assert.AreEqual(products.Length, 3);
            Assert.AreEqual("p1", products[0].Name);
            Assert.AreEqual("p2", products[1].Name);
            Assert.AreEqual("p3", products[2].Name);
        }

        [Test]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "p1"},
                new Product {Id = 2, Name = "p2"},
                new Product {Id = 3, Name = "p3"},
            });

            AdminController controller = new AdminController(mock.Object);

            Product p1 = controller.Edit(1).ViewData.Model as Product;
            Product p2 = controller.Edit(2).ViewData.Model as Product;
            Product p3 = controller.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.Id);
            Assert.AreEqual(2, p2.Id);
            Assert.AreEqual(3, p3.Id);
        }

        [Test]
        public void Can_Not_Edit_Non_Existent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "p1"},
                new Product {Id = 2, Name = "p2"},
                new Product {Id = 3, Name = "p3"},
            });

            AdminController controller = new AdminController(mock.Object);

            Product res = (Product) controller.Edit(4).ViewData.Model;

            Assert.IsNull(res);
        }

        [Test]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            controller.TempData = tempData;

            Product product = new Product {Name = "Test"};

            //Act
            IActionResult result = controller.Edit(product);

            //Assert
            // check if have saved product to repo 
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOf<ViewResult>(result);
            Assert.IsTrue(tempData.ContainsKey("message"));
        }

        [Test]
        public void Can_Not_Save_Invalid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController controller = new AdminController(mock.Object);

            Product product = new Product
            {
                Name = "Test"
            };

            // Добавление ошибочного состояния
            controller.ModelState.AddModelError("error", "error");

            IActionResult result = controller.Edit(product);

            // проверка того, что обращения к хранилищу не будет
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // проверка, что метод вернул ViewResult, а не редирект, как должно быть при валидных данных
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}