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
    private Mock<IAuthorizationService> _mockAuthService;
    private Mock<INotificationService> _mockNotificationService;
    private StoreService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IStoreRepository>();
        _mockAuthService = new Mock<IAuthorizationService>();
        _mockNotificationService = new Mock<INotificationService>();

        _service = new StoreService(
            _mockRepository.Object,
            _mockAuthService.Object,
            _mockNotificationService.Object
        );
    }

    [TestMethod]
    public async Task CreateUser_ReturnsUser_WhenCreated()
    {
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", UserType.Customer);

        _mockRepository.Setup(r => r.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        var result = await _service.CreateUserAsync(user);

        Assert.IsNotNull(result);
        Assert.AreEqual(user.Email, result.Email);
        _mockRepository.Verify(r => r.CreateUserAsync(user), Times.Once);
    }

    [TestMethod]
    public async Task CreateUser_ThrowsException_WhenAlreadyExists()
    {
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", UserType.Customer);

        _mockRepository.Setup(r => r.FindUserByDocumentOrEmailAsync(user.Document, user.Email))
            .ReturnsAsync(user);

        var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.CreateUserAsync(user));

        Assert.AreEqual("User with same document or email already exists.", ex.Message);
    }

    [TestMethod]
    public async Task TransferAsync_Successful()
    {
        var payer = new User("payer", "doc1", "payer@test.com", "123", UserType.Customer);
        var payee = new User("payee", "doc2", "payee@test.com", "123", UserType.Customer);
        payer.AddBalance(1000);

        _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(payer);
        _mockRepository.Setup(r => r.GetAsync(2)).ReturnsAsync(payee);
        _mockRepository.Setup(r => r.TransferAsync(It.IsAny<User>(), It.IsAny<User>())).ReturnsAsync(true);
        _mockAuthService.Setup(a => a.IsTransferAuthorizedAsync()).ReturnsAsync(true);
        _mockNotificationService.Setup(n => n.SendNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(true);

        await _service.TransferAsync(100, 1, 2);

        _mockRepository.Verify(r => r.TransferAsync(payer, payee), Times.Once);
        _mockNotificationService.Verify(n => n.SendNotificationAsync(It.IsAny<Notification>()), Times.Exactly(2));
    }

    [TestMethod]
    public async Task TransferAsync_Throws_WhenNotAuthorized()
    {
        var payer = new User("payer", "doc1", "payer@test.com", "MyP@ssW0rD", UserType.Customer);
        var payee = new User("payee", "doc2", "payee@test.com", "MyP@ssW0rD", UserType.Customer);

        payer.AddBalance(100);

        _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(payer);
        _mockRepository.Setup(r => r.GetAsync(2)).ReturnsAsync(payee);
        _mockAuthService.Setup(a => a.IsTransferAuthorizedAsync()).ReturnsAsync(false);

        var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.TransferAsync(100, 1, 2));

        Assert.AreEqual("Transfer is not authorized. Please try again later", ex.Message);
    }

    [TestMethod]
    public async Task TransferAsync_Throws_WhenTransferFails()
    {
        var payer = new User("payer", "doc1", "payer@test.com", "123", UserType.Customer);
        var payee = new User("payee", "doc2", "payee@test.com", "123", UserType.Customer);
        payer.AddBalance(1000);

        _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(payer);
        _mockRepository.Setup(r => r.GetAsync(2)).ReturnsAsync(payee);
        _mockRepository.Setup(r => r.TransferAsync(It.IsAny<User>(), It.IsAny<User>())).ReturnsAsync(false);
        _mockAuthService.Setup(a => a.IsTransferAuthorizedAsync()).ReturnsAsync(true);

        var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.TransferAsync(100, 1, 2));

        Assert.AreEqual("An error ocurred while transfering. Please try again later", ex.Message);
    }

    [TestMethod]
    public async Task DepositAsync_Successful()
    {
        var user = new User("name", "doc", "email@test.com", "MyP@ssW0rD", UserType.Customer);
        user.HashPassword();

        var deposit = new Deposit
        {
            Document = "doc",
            Email = "email@test.com",
            Password = "MyP@ssW0rD",
            Value = 500
        };

        _mockRepository.Setup(r => r.FindUserByDocumentOrEmailAsync("doc", "email@test.com")).ReturnsAsync(user);

        await _service.DepositAsync(deposit);

        _mockRepository.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [TestMethod]
    public async Task DepositAsync_Throws_WhenUserNotFound()
    {
        var deposit = new Deposit
        {
            Document = "doc",
            Email = "email@test.com",
            Password = "pwd",
            Value = 500
        };

        _mockRepository.Setup(r => r.FindUserByDocumentOrEmailAsync("doc", "email@test.com")).ReturnsAsync((User)null);

        var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _service.DepositAsync(deposit));

        Assert.AreEqual("User not found.", ex.ParamName);
    }
}
