using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using GameStore.WebUI.Models;
using GameStore.WebUI.HtmlHelpers;


namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[]
                {
                    new Product {productID = 1, name = "P1"},
                    new Product {productID = 2, name = "P2"},
                    new Product {productID = 3, name = "P3"},
                    new Product {productID = 4, name = "P4"},
                    new Product {productID = 5, name = "P5"}
                 });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //ACT
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("P4", prodArray[0].name);
            Assert.AreEqual("P5", prodArray[1].name);
        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            //Arrange
            HtmlHelper myHelper = null;
            var pagingInfo = new PagingInfo
            {
                currentPage = 2,
                totalItems = 28,
                itemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = (i => "Page" + i);

            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
            @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]

        public void CanSendPaginationViewModel()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[]
                {
                    new Product {productID = 1, name = "P1"},
                    new Product {productID = 2, name = "P2"},
                    new Product {productID = 3, name = "P3"},
                    new Product {productID = 4, name = "P4"},
                    new Product {productID = 5, name = "P5"}
                 });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //ACT
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            PagingInfo pageInfo = result.pagingInfo;
            Assert.AreEqual(2, pageInfo.currentPage);
            Assert.AreEqual(3, pageInfo.itemsPerPage);
            Assert.AreEqual(5, pageInfo.totalItems);
            Assert.AreEqual(2, pageInfo.totalPages);
        }

        [TestMethod]

        public void CanFilterProducts()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[]
                {
                    new Product {productID = 1, name = "P1", category = "Cat1"},
                    new Product {productID = 2, name = "P2", category = "Cat2"},
                    new Product {productID = 3, name = "P3", category = "Cat1"},
                    new Product {productID = 4, name = "P4", category = "Cat2"},
                    new Product {productID = 5, name = "P5", category = "Cat1"}
                 });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //ACT
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .products.ToArray();

            //Assert
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result[0].name == "P2" && result[0].category == "Cat2");
            Assert.IsTrue(result[1].name == "P4" && result[0].category == "Cat2");
        }

        [TestMethod]
        public void CanCreateCategories()
        {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[] {
                new Product {productID = 1, name = "P1", category = "Oranges"},
                new Product {productID = 2, name = "P2", category = "Plums"},
                new Product {productID = 3, name = "P3", category = "Apples"},
                new Product {productID = 4, name = "P4", category = "Apples"},
            });                                          

            // Arrange - create the controller
            NavController target = new NavController(mock.Object);

            // Act = get the set of categories 
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void IndicatesSelectedCategory()
        {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[] {
                new Product {productID = 1, name = "P1", category = "Apples"},
                new Product {productID = 4, name = "P2", category = "Oranges"},
            });

            // Arrange - create the controller 
            NavController target = new NavController(mock.Object);

            // Arrange - define the category to selected
            string categoryToSelect = "Apples";

            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void GenerateCategorySpecificProductCount()
        {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[] {
                new Product {productID = 1, name = "P1", category = "Cat1"},
                new Product {productID = 2, name = "P2", category = "Cat2"},
                new Product {productID = 3, name = "P3", category = "Cat1"},
                new Product {productID = 4, name = "P4", category = "Cat2"},
                new Product {productID = 5, name = "P5", category = "Cat3"}
            });

            // Arrange - create a controller and make the page size 3 items
            ProductController target = new ProductController(mock.Object);
            target.pageSize = 3;

            // Action - test the product counts for different categories
            int res1 = ((ProductsListViewModel)target
                .List("Cat1").Model).pagingInfo.totalItems;
            int res2 = ((ProductsListViewModel)target
                .List("Cat2").Model).pagingInfo.totalItems;
            int res3 = ((ProductsListViewModel)target
                .List("Cat3").Model).pagingInfo.totalItems;
            int resAll = ((ProductsListViewModel)target
                .List(null).Model).pagingInfo.totalItems;

            // Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
