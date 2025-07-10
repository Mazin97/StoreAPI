using Domain.Extensions;

namespace StoreAPI.Tests.Helpers;

[TestClass]
public class EmailHelperTests
{
    [TestMethod]
    public void IsValid_WithValidEmail_ReturnsTrue()
    {
        var email = "test.user@example.com";
        var result = EmailHelper.IsValid(email);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValid_WithNullEmail_ReturnsFalse()
    {
        string email = null;
        var result = EmailHelper.IsValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithEmptyEmail_ReturnsFalse()
    {
        var result = EmailHelper.IsValid(string.Empty);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithoutAtSymbol_ReturnsFalse()
    {
        var email = "invalid.email.com";
        var result = EmailHelper.IsValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithMultipleAtSymbols_ReturnsFalse()
    {
        var email = "user@@example.com";
        var result = EmailHelper.IsValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithoutDot_ReturnsFalse()
    {
        var email = "user@examplecom";
        var result = EmailHelper.IsValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithInvalidCharacters_ReturnsFalse()
    {
        var email = "user<>@example.com";
        var result = EmailHelper.IsValid(email);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithValidSubdomain_ReturnsTrue()
    {
        var email = "user@mail.example.com";
        var result = EmailHelper.IsValid(email);
        Assert.IsTrue(result);
    }
}

