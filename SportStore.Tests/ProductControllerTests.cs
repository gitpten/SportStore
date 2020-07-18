using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using SportStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //
            ProductListViewModel result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            //
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        private static Mock<IProductRepository> GetMock()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "C1" },
                new Product { ProductID = 2, Name = "P2",Category = "C2" },
                new Product { ProductID = 3, Name = "P3",Category = "C1" },
                new Product { ProductID = 4, Name = "P4",Category = "C2" },
                new Product { ProductID = 5, Name = "P5",Category = "C3" },
            }.AsQueryable<Product>());
            return mock;
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //
            ProductListViewModel result = controller.List(null, 2).ViewData.Model as ProductListViewModel;


            //
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //
            Mock<IProductRepository> mock = GetMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //
            Product[] result = (controller.List("C2", 1).ViewData.Model as ProductListViewModel).Products.ToArray();

            //
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "C2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "C2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = GetMock();
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Func<ViewResult, ProductListViewModel> GetModel = result => result?.ViewData?.Model as ProductListViewModel;

            //
            int? res1 = GetModel(controller.List("C1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(controller.List("C2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(controller.List("C3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(controller.List(null))?.PagingInfo.TotalItems;

            //
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
