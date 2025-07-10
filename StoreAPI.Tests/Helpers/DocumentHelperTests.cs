using Domain.Extensions;

namespace StoreAPI.Tests.Helpers;

[TestClass]
public class DocumentHelperTests
{
    [TestMethod]
    public void IsValid_WithValidCpf_ReturnsTrue()
    {
        var validCpf = "52998224725"; // known valid CPF
        var result = DocumentHelper.IsValid(validCpf);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValid_WithInvalidCpf_ReturnsFalse()
    {
        var invalidCpf = "12345678900";
        var result = DocumentHelper.IsValid(invalidCpf);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithValidCnpj_ReturnsTrue()
    {
        var validCnpj = "11222333000181"; // known valid CNPJ
        var result = DocumentHelper.IsValid(validCnpj);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValid_WithInvalidCnpj_ReturnsFalse()
    {
        var invalidCnpj = "11222333000100";
        var result = DocumentHelper.IsValid(invalidCnpj);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithCpfAllSameDigits_ReturnsFalse()
    {
        var cpf = "11111111111";
        var result = DocumentHelper.IsValid(cpf);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_WithCnpjAllSameDigits_ReturnsFalse()
    {
        var cnpj = "00000000000000";
        var result = DocumentHelper.IsValid(cnpj);
        Assert.IsFalse(result);
    }
}
