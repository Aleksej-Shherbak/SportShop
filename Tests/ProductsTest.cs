using System;
using System.Linq;
using Domains.Abstract;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using Domains.Entities;
using Web.Controllers;
using Web.HtmlHelpers;
using Web.Models;

namespace Tests
{
    public class ProductsTest
    {
        private IHtmlHelper _htmlHelper;
        private PaginInfo _paginInfo;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Pagination_Works()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product
                {
                    Name = "P1",
                    Id = 1,
                },
                new Product
                {
                    Name = "P2",
                    Id = 2,
                },
                new Product
                {
                    Name = "P3",
                    Id = 3,
                },
                new Product
                {
                    Name = "P4",
                    Id = 4,
                },
                new Product
                {
                    Name = "P5",
                    Id = 5
                }
            });

            var controller = new ProductsController(mock.Object) {PageSize = 3};

            var result = (ProductsListViewModel) controller.List(null,2).Model;

            var productsArray = result.Products.ToArray();

            Assert.IsTrue(productsArray.Length == 2);
            Assert.AreEqual(productsArray[0].Name, "P4");
            Assert.AreEqual(productsArray[1].Name, "P5");
        }

        [Test]
        public void Can_Generate_Page_Links()
        {
            var paginInfo = new PaginInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page " + i;

            HtmlString result = _htmlHelper.PageLinks(paginInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page 1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page 2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page 3"">3</a>", result.ToString());
        }


        [Test]
        public void Can_Send_Pagination_View_Model()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2"},
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            });

            var controller = new ProductsController(mock.Object) {PageSize = 3};

            var result = (ProductsListViewModel) controller.List(null, 2).Model;

            var paginInfo = result.PaginInfo;

            Assert.AreEqual(paginInfo.CurrentPage, 2);
            Assert.AreEqual(paginInfo.ItemsPerPage, 3);
            Assert.AreEqual(paginInfo.TotalItems, 5);
            Assert.AreEqual(paginInfo.TotalPages, 2);
        }
    }
}