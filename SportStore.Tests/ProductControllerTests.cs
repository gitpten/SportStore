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
            ProductListViewModel result = controller.List(2).ViewData.Model as ProductListViewModel;

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
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" },
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
            ProductListViewModel result = controller.List(2).ViewData.Model as ProductListViewModel;


            //
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }
    }
}
