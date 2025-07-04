using Domain.Models;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Tests.Repositories;

[TestClass]
public class StoreRepositoryTests
{
    private static StoreContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<StoreContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new StoreContext(options);
    }

    [TestMethod]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new StoreRepository(context);
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", Domain.Enums.UserTypeEnum.Client);

        // Act
        var result = await repository.CreateUserAsync(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Name, result.Name);
        Assert.AreEqual(user.Document, result.Document);
        Assert.AreEqual(user.Email, result.Email);
        Assert.AreEqual(user.Password, result.Password);
        Assert.AreEqual(user.Type, result.Type);
        Assert.AreEqual(user.Balance, result.Balance);
    }

    [TestMethod]
    public async Task FindUserByDocumentOrEmail_ReturnsUser_WhenFindingByDocument()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new StoreRepository(context);
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", Domain.Enums.UserTypeEnum.Client);
        await repository.CreateUserAsync(user);

        // Act
        var result = await repository.FindUserByDocumentOrEmailAsync("61399642022", string.Empty);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Name, result.Name);
        Assert.AreEqual(user.Document, result.Document);
        Assert.AreEqual(user.Email, result.Email);
        Assert.AreEqual(user.Password, result.Password);
        Assert.AreEqual(user.Type, result.Type);
        Assert.AreEqual(user.Balance, result.Balance);
    }

    [TestMethod]
    public async Task FindUserByDocumentOrEmail_ReturnsUser_WhenFindingByEmail()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repository = new StoreRepository(context);
        var user = new User("John Doe", "61399642022", "john.doe@test.com", "MyP@ssW0rD", Domain.Enums.UserTypeEnum.Client);
        await repository.CreateUserAsync(user);

        // Act
        var result = await repository.FindUserByDocumentOrEmailAsync(string.Empty, "john.doe@test.com");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Name, result.Name);
        Assert.AreEqual(user.Document, result.Document);
        Assert.AreEqual(user.Email, result.Email);
        Assert.AreEqual(user.Password, result.Password);
        Assert.AreEqual(user.Type, result.Type);
        Assert.AreEqual(user.Balance, result.Balance);
    }
}
