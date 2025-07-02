namespace Domain.Extensions;

public static class PasswordHelper
{
    public static bool IsValid(string password, int minLength = 8, int maxLength = 128)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < minLength || password.Length > maxLength)
            return false;

        bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;

        foreach (var ch in password)
        {
            if (char.IsUpper(ch)) hasUpper = true;
            else if (char.IsLower(ch)) hasLower = true;
            else if (char.IsDigit(ch)) hasDigit = true;
            else if (!char.IsWhiteSpace(ch)) hasSpecial = true;
        }

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}
