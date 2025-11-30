using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AbysaltoTaskTests
{
    [TestClass]
    public class CartControllerTests
    {
        private Mock<ICartRepository> _cartRepo = null!;
        private CartController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _cartRepo = new Mock<ICartRepository>();
            _controller = new CartController(_cartRepo.Object);
        }

        [TestMethod]
        public async Task GetMyCart_ReturnsOk_WhenCartExists()
        {
            // Arrange
            _cartRepo.Setup(r => r.GetCartForUserAsync(It.IsAny<int>())).ReturnsAsync(new CartResponse { Id = 1, Articles = new List<ArticleDto>() });

            // Act
            var result = await _controller.GetMyCart();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
    }
}