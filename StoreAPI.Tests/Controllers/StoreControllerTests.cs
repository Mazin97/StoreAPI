using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StoreAPI.Controllers;

namespace StoreAPI.Tests.Controllers;

[TestClass]
public class StoreControllerTests
{
    private Mock<IStoreService> _mockService;
    private Mock<ILogger<StoreController>> _mockLogger;
    private StoreController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IStoreService>();
        _mockLogger = new Mock<ILogger<StoreController>>();
        _controller = new StoreController(_mockLogger.Object, _mockService.Object);
    }

    [TestMethod]
    public async Task CreateUserAsync_WithValidUser_ReturnsOk()
    {
        var user = new User("Test", "52998224725", "test@mail.com", "aA1@Strong", UserType.Customer);
        _mockService.Setup(s => s.CreateUserAsync(user)).ReturnsAsync(user);

        var result = await _controller.CreateUserAsync(user) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task CreateUserAsync_WhenServiceThrows_Returns500()
    {
        var user = new User("Test", "52998224725", "test@mail.com", "aA1@Strong", UserType.Customer);
        _mockService.Setup(s => s.CreateUserAsync(user)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.CreateUserAsync(user) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.StatusCode);
    }
}
