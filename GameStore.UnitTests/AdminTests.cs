
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void IndexContainsAllProducts()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[]
                {
                    new Product {productID = 1, name = "P1"},
                    new Product {productID = 2, name = "P2"},
                    new Product {productID = 3, name = "P3"},
                 });

            var target = new AdminController(mock.Object);

            //Action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("P1", result[0].name);
            Assert.AreEqual("P2", result[1].name);
            Assert.AreEqual("P3", result[2].name);
        }

        [TestMethod]

        public void CanEditProduct()
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

            var target = new AdminController(mock.Object);

            //Act
            Product p1 = target.Edit(1).ViewData.Model as Product;

            //Assert
            Assert.AreEqual(1, p1.productID);
        }

        [TestMethod]

        public void CannotEditNonexistentProduct()
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

            var target = new AdminController(mock.Object);

            //Act
            Product result = (Product)target.Edit(6).ViewData.Model;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]

        public void CanSaveValidChanges()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            var target = new AdminController(mock.Object);
            var product = new Product { name = "Test" };

            //Act
            ActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]

        public void CannotSaveInvalidChanges()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            var target = new AdminController(mock.Object);
            var product = new Product { name = "Test" };
            target.ModelState.AddModelError("Error", "Error");

            //Act
            ActionResult result = target.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]

        public void CanDeleteValidProducts()
        {
            //Arrage
            var product = new Product { productID = 2, name = "Twst" };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[]
                {
                    new Product {productID = 1, name = "P1"},
                    product,
                    new Product {productID = 3, name = "P3"}
                });

            var target = new AdminController(mock.Object);

            //Act
            target.Delete(product.productID);

            //Assert
            mock.Verify(m => m.DeleteProduct(product.productID));
        }
    }
}
