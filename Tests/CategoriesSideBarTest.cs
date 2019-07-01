using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Web.ViewComponents;

namespace Tests
{
    public class CategoriesSideBarTest
    {
        [Test]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 2, Name = "P2", Category = "Apples"},
                new Product {Id = 3, Name = "P3", Category = "Plums"},
                new Product {Id = 4, Name = "P4", Category = "Oranges"},
            });

            CategoryNav categoryNav = new CategoryNav(mock.Object);

            var results = ((IEnumerable<string>) ((ViewViewComponentResult) categoryNav.Invoke("Plums")).ViewData.Model)
                .ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [Test]
        public void Indicate_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 4, Name = "P2", Category = "Oranges"},
            });

            CategoryNav categoryNav = new CategoryNav(mock.Object);
            
            var categoryToSelect = "Appless";

            var result = (ViewViewComponentResult) categoryNav.Invoke(categoryToSelect);

            Assert.AreEqual(categoryToSelect, result.ViewData["SelectedCategory"]);
        }
    }
}