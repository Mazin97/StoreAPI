using System.Text.Json.Serialization;

namespace Domain.Models;

public class Transfer
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("payer")]
    public int Payer { get; set; }

    [JsonPropertyName("payee")]
    public int Payee { get; set; }

    public static void Validate(User payer, User payee, decimal value)
    {
        ArgumentNullException.ThrowIfNull(payer, "Payer is required.");
        ArgumentNullException.ThrowIfNull(payee, "Payee is required.");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, "Transfer value must be greater than zero.");

        if (payer.Balance < value)
            throw new InvalidOperationException($"Insufficient balance. Payer has {payer.Balance}, attempted to transfer {value}.");

        if (payer.Type == Enums.UserType.Owner)
            throw new InvalidOperationException("Transfers are not allowed for users of type 'Owner'.");
    }
}
