using Domain.Enums;
using Domain.Models;

namespace StoreAPI.Tests.Domain;

[TestClass]
public class UserTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Name_Is_Null()
    {
        var user = new User(null, "12345678901", "test@mail.com", "Pass@123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Document_Is_Null()
    {
        var user = new User("Test", null, "test@mail.com", "Pass@123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validate_Throws_When_Document_Is_Invalid()
    {
        var user = new User("Test", "invalid-doc", "test@mail.com", "Pass@123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Email_Is_Null()
    {
        var user = new User("Test", "11088522033", null, "Pass@123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validate_Throws_When_Email_Is_Invalid()
    {
        var user = new User("Test", "11088522033", "invalid-email", "Pass@123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Password_Is_Null()
    {
        var user = new User("Test", "11088522033", "test@mail.com", null, UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validate_Throws_When_Password_Is_Invalid()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "123", UserType.Customer);
        user.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Type_Is_None()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.None);
        user.Validate();
    }

    [TestMethod]
    public void HashPassword_Replaces_Password_With_Hashed()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.Customer);
        var original = user.Password;

        user.HashPassword();

        Assert.AreNotEqual(original, user.Password);
    }

    [TestMethod]
    public void SetId_Assigns_Id()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.Customer);
        user.SetId(42);
        Assert.AreEqual(42, user.Id);
    }

    [TestMethod]
    public void AddBalance_Increases_Balance()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.Customer);
        user.AddBalance(100);
        Assert.AreEqual(100, user.Balance);
    }

    [TestMethod]
    public void RemoveBalance_Decreases_Balance()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.Customer);
        user.AddBalance(100);
        user.RemoveBalance(40);
        Assert.AreEqual(60, user.Balance);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RemoveBalance_Throws_When_Insufficient()
    {
        var user = new User("Test", "11088522033", "test@mail.com", "Pass@123", UserType.Customer);
        user.RemoveBalance(10);
    }
}

