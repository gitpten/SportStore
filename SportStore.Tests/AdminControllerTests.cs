using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            AdminController controller = new AdminController(mock.Object);

            //
            Product[] result = GetViewModel<IEnumerable<Product>>(controller.Index())?.ToArray();

            //
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            AdminController controller = new AdminController(mock.Object);

            //
            Product p1 = GetViewModel<Product>(controller.Edit(1));
            Product p2 = GetViewModel<Product>(controller.Edit(2));
            Product p3 = GetViewModel<Product>(controller.Edit(3));

            //
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            AdminController controller = new AdminController(mock.Object);

            //
            Product result = GetViewModel<Product>(controller.Edit(4));

            Assert.Null(result);
        }

        private static Mock<IProductRepository> GetMock()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" }
            }).AsQueryable<Product>());
            return mock;
        }

        private T GetViewModel<T>(IActionResult result)
            where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
