using Domain.Models;

namespace StoreAPI.Tests.Domain;

[TestClass]
public class DepositTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Email_Is_Null()
    {
        var Deposit = new Deposit()
        {
            Email = null,
            Document = "11088522033",
            Password = "MyPa@ss321",
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Email_Is_Empty()
    {
        var Deposit = new Deposit()
        {
            Email = string.Empty,
            Document = "11088522033",
            Password = "MyPa@ss321",
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Document_Is_Null()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = null,
            Password = "MyPa@ss321",
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Document_Is_Empty()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = string.Empty,
            Password = "MyPa@ss321",
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Password_Is_Null()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = "11088522033",
            Password = null,
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Password_Is_Empty()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = "11088522033",
            Password = string.Empty,
            Value = 100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Validate_Throws_When_ValueIsNegative()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = "11088522033",
            Password = "MyPa@ss321",
            Value = -100.0M
        };

        Deposit.Validate();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Validate_Throws_When_ValueIsZero()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = "11088522033",
            Password = "MyPa@ss321",
            Value = -100.0M
        };

        Deposit.Validate();
    }
    [TestMethod]
    public void Validate_Works()
    {
        var Deposit = new Deposit()
        {
            Email = "myemail@test.com",
            Document = "11088522033",
            Password = "MyPa@ss321",
            Value = 100.0M
        };

        Deposit.Validate();
    }

}
