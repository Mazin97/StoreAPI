using System.Text.Json.Serialization;

namespace Domain.Models;

public class Transfer
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("payer")]
    public int Payer { get; set; }

    [JsonPropertyName("payee")]
    public int Payee { get; set; }

    public static void Validate(User payer, User payee, decimal value)
    {
        ArgumentNullException.ThrowIfNull(payer);
        ArgumentNullException.ThrowIfNull(payee);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

        if (payer.Balance < value)
            throw new InvalidOperationException("Payer does not have enought balance to transfer.");

        if (payer.Type == Enums.UserType.Owner)
            throw new InvalidOperationException("Owners cannot do transfers.");
    }
}
