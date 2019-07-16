using System.Collections.Generic;
using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Moq;
using NUnit.Framework;
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
    }
}