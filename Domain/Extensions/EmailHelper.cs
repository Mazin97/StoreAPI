using System.Text.RegularExpressions;

namespace Domain.Extensions;
public static class EmailHelper
{
    private static readonly Regex _emailRegex = new(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

    public static bool IsValid(string email)
    {
        if (string.IsNullOrEmpty(email)) return false;

        if (!email.Contains('@')) return false;

        if (!email.Split('@').Length.Equals(2)) return false;

        if (!email.Contains('.')) return false;

        return _emailRegex.IsMatch(email);
    }
}
