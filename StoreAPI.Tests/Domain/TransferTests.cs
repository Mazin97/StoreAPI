using Domain.Enums;
using Domain.Models;

namespace StoreAPI.Tests.Domain;

[TestClass]
public class TransferTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Payer_Is_Null()
    {
        Transfer.Validate(null, new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), 10);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Validate_Throws_When_Payee_Is_Null()
    {
        Transfer.Validate(new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), null, 10);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Validate_Throws_When_Value_Is_Zero()
    {
        Transfer.Validate(new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Validate_Throws_When_Value_Is_Negative()
    {
        Transfer.Validate(new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None), -5);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Validate_Throws_When_Balance_Is_Insufficient()
    {
        var payer = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None);

        payer.AddBalance(5);

        var payee = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None);
        Transfer.Validate(payer, payee, 10);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Validate_Throws_When_Payer_Is_Owner()
    {
        var payer = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.Owner);
        payer.AddBalance(100);

        var payee = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None);
        Transfer.Validate(payer, payee, 50);
    }

    [TestMethod]
    public void Validate_Passes_When_Valid()
    {
        var payer = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.Customer);
        payer.AddBalance(100);

        var payee = new User(string.Empty, string.Empty, string.Empty, string.Empty, UserType.None);
        Transfer.Validate(payer, payee, 50);
    }
}
