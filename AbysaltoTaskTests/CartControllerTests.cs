using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

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

        //Da ima neki test
        [TestMethod]
        public async Task GetMyCart_ReturnsOk_WhenCartExists()
        {
            // Arrange
            _cartRepo.Setup(r => r.GetCartForUserAsync(It.IsAny<int>()))
                .ReturnsAsync(new CartResponse { Id = 1, Articles = new List<ArticleDto>() });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "5")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.GetMyCart();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
    }
}