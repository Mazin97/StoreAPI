namespace Domain.Extensions;

public static class DocumentHelper
{
    public static bool IsValid(string document)
    {
        if (document.Length <= 13)
            return IsValidCpf(document);

        return IsValidCnpj(document);
    }

    private static bool IsValidCpf(string cpf)
    {
        cpf = OnlyDigits(cpf);
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        var digits = cpf.Select(c => c - '0').ToArray();

        return CheckCpfDigit(digits, 9) && CheckCpfDigit(digits, 10);
    }

    private static bool IsValidCnpj(string cnpj)
    {
        cnpj = OnlyDigits(cnpj);
        if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
            return false;

        var digits = cnpj.Select(c => c - '0').ToArray();
        return CheckCnpjDigit(digits, 12) && CheckCnpjDigit(digits, 13);
    }

    private static string OnlyDigits(string input) => new(input.Where(char.IsDigit).ToArray());

    private static bool CheckCpfDigit(int[] digits, int position)
    {
        var sum = 0;
        
        for (int i = 0; i < position; i++)
            sum += digits[i] * (position + 1 - i);

        var check = sum * 10 % 11;
        if (check == 10) check = 0;

        return digits[position] == check;
    }

    private static bool CheckCnpjDigit(int[] digits, int position)
    {
        int[] weights = position == 12
            ? [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]
            : [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = 0;
        for (int i = 0; i < weights.Length; i++)
            sum += digits[i] * weights[i];

        var check = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        return digits[position] == check;
    }
}
