using Domain.Enums;
using Domain.Extensions;

namespace Domain.Models;

public sealed class User(string name, string document, string email, string password, UserType type)
{
    public int Id { get; private set; } = default;

    public string Name { get; private set; } = name;

    public string Document { get; private set; } = document;

    public string Email { get; private set; } = email;

    public string Password { get; private set; } = password;

    public UserType Type { get; private set; } = type;

    public decimal Balance { get; private set; } = default;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException(nameof(Name));

        if (string.IsNullOrWhiteSpace(Document)) throw new ArgumentNullException(nameof(Document));

        if (!DocumentHelper.IsValid(Document)) throw new ArgumentException($"Invalid document");

        if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException(nameof(Email));

        if (!EmailHelper.IsValid(Email)) throw new ArgumentException("Invalid email");

        if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentNullException(nameof(Password));

        if (!PasswordHelper.IsValid(Password)) throw new ArgumentException("Invalid password, a password must contains at least 8 characters, a special character, upper and lower case letters");

        if (Type == UserType.None) throw new ArgumentNullException(nameof(Type));
    }

    /// <summary>
    /// Hashes the current plain-text password and replaces the Password property.
    /// </summary>
    public void HashPassword()
    {
        Password = PasswordHelper.HashPassword(Password);
    }

    public void SetId(int id)
    {
        Id = id;
    }

    public void AddBalance(decimal amount)
    {
        Balance += amount;
    }

    public void RemoveBalance(decimal amount)
    {
        if (Balance < amount)
            throw new InvalidOperationException("The balance hasnt the amount tried to be removed.");

        Balance -= amount;
    }
}
