using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Service.Store;

namespace StoreAPI.Tests.Services;

[TestClass]
public class StoreServiceTests
{
    private Mock<IStoreRepository> _mockRepository;
    private StoreService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IStoreRepository>();
        _service = new StoreService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task CreateUser_ReturnsUser_WhenCreated()
    {
        // Arrange
        var expectedUser = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", UserType.Customer);

        _mockRepository.Setup(r => r.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _service.CreateUserAsync(expectedUser);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUser.Name, result.Name);
        Assert.AreEqual(expectedUser.Document, result.Document);
        Assert.AreEqual(expectedUser.Email, result.Email);
        Assert.AreEqual(expectedUser.Password, result.Password);
        Assert.AreEqual(expectedUser.Type, result.Type);
        Assert.AreEqual(expectedUser.Balance, result.Balance);
        _mockRepository.Verify(r => r.CreateUserAsync(expectedUser), Times.Once);
    }

    [TestMethod]
    public async Task CreateUser_ThrowsException_WhenAlreadyExists()
    {
        // Arrange
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", UserType.Customer);

        _mockRepository.Setup(r => r.FindUserByDocumentOrEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.CreateUserAsync(user));
        Assert.AreEqual("User with same document or email already exists.", ex.Message);
    }
}
