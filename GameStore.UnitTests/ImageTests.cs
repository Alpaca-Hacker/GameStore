using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests {
    [TestClass]
    public class ImageTests {

        [TestMethod]
        public void Can_Retrieve_Image_Data() {

            // Arrange - create a Product with image data
            Product prod = new Product {
                productID = 2,
                name = "Test",
                imageData = new byte[] { },
                imageMimeType = "image/png"
            };

            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[] {
                new Product {productID = 1, name = "P1"},
                prod,                            
                new Product {productID = 3, name = "P3"}
            }.AsQueryable());

            // Arrange - create the controller
            ProductController target = new ProductController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.imageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID() {

            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.products).Returns(new Product[] {
                new Product {productID = 1, name = "P1"},
                new Product {productID = 2, name = "P2"}
            }.AsQueryable());

            // Arrange - create the controller
            ProductController target = new ProductController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(100);

            // Assert
            Assert.IsNull(result);
        }
    }
}
