using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Moq;
using NUnit.Framework;
using Web.Controllers;
using Web.Models;

namespace Tests
{
    public class ProductCategoryTest
    {
        [Test]
        public void Can_Filter_Products()
        {
            // organization 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat1"},
                new Product {Id = 2, Name = "P2", Category = "Cat2"},
                new Product {Id = 3, Name = "P3", Category = "Cat1"},
                new Product {Id = 4, Name = "P4", Category = "Cat2"},
                new Product {Id = 5, Name = "P5", Category = "Cat3"}
            });

            ProductsController controller = new ProductsController(mock.Object);
            controller.PageSize = 3;

            // action
            Product[] result = ((ProductsListViewModel) controller.List("Cat2", 1).Model)
                .Products.ToArray();
            
            // asserts 
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }
    }
}