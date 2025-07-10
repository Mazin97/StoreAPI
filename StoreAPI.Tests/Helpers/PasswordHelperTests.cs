using Domain.Extensions;

namespace StoreAPI.Tests.Helpers;

[TestClass]
public class PasswordHelperTests
{
    [TestMethod]
    public void IsValid_WithValidPassword_ReturnsTrue()
    {
        var result = PasswordHelper.IsValid("Aa1@strong");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValid_WithNoUppercase_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("aa1@lowercase");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithNoLowercase_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("AA1@UPPER");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithNoDigit_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("Aa@Password");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithNoSpecialChar_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("Aa1Password");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithShortPassword_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("Aa1@");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithLongPassword_ReturnsFalse()
    {
        var longPassword = new string('A', 129) + "a1@";
        var result = PasswordHelper.IsValid(longPassword);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithWhitespacePassword_ReturnsFalse()
    {
        var result = PasswordHelper.IsValid("     ");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void HashPassword_ProducesDeterministicHash()
    {
        var hash1 = PasswordHelper.HashPassword("MyPassword123!");
        var hash2 = PasswordHelper.HashPassword("MyPassword123!");
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void HashPassword_WithDifferentInputs_ProducesDifferentHashes()
    {
        var hash1 = PasswordHelper.HashPassword("Password1!");
        var hash2 = PasswordHelper.HashPassword("Password2!");
        Assert.AreNotEqual(hash1, hash2);
    }
}
