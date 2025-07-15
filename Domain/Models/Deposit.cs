using System.Text.Json.Serialization;

namespace Domain.Models;

public class Deposit
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentNullException(nameof(Email));

        if (string.IsNullOrWhiteSpace(Document))
            throw new ArgumentNullException(nameof(Document));

        if (string.IsNullOrWhiteSpace(Password))
            throw new ArgumentNullException(nameof(Password));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Value, "Deposit value must be greater than zero.");
    }
}
