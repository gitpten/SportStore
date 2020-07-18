
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportStore.Components;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Routing;

namespace SportStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //
            string[] result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            //
            Assert.True(Enumerable.SequenceEqual(new string[] { "One", "Three", "Two" }, result));
        }

        private static Mock<IProductRepository> GetMock()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "One" },
                new Product { ProductID = 2, Name = "P2",Category = "Two" },
                new Product { ProductID = 3, Name = "P3",Category = "One" },
                new Product { ProductID = 4, Name = "P4",Category = "One" },
                new Product { ProductID = 5, Name = "P5",Category = "Three" },
            }.AsQueryable<Product>());
            return mock;
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //
            string categoryToSelect = "One";
            Mock<IProductRepository> mock = GetMock();
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext { RouteData = new RouteData() }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            //
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //
            Assert.Equal(categoryToSelect, result);

        }
    }
}