using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Utils;

internal static partial class Validator
{
    public static bool IsValidURL(string url, out string result)
    {
        bool isValid = false;
        result = string.Empty;

        // Check for null, empty, or whitespace string
        if (string.IsNullOrWhiteSpace(url))
            return isValid;

        // Validating URL using the generated regex
        if (GetUrlRegex().IsMatch(url))
        {
            isValid = true;
            result = url.Trim();
        }

        return isValid;
    }

    public static bool IsValidEmail(string email)
    {
        // if email is null, empty or white space then return false
        if (string.IsNullOrWhiteSpace(email))
            return false;

        email = email.Trim().ToLower();

        // Validating email using the generated regex
        return GetEmailRegex().IsMatch(email);
    }

    // Generated regex for validating URLs
    [GeneratedRegex(@"^https?:\/\/([a-zA-Z0-9-\.]+)(\.[a-zA-Z]{2,})(\:\d+)?(\/[\w\-\.~]*)*(\?[;&a-zA-Z0-9\-\.=_~%]*)?(\#[-a-zA-Z0-9_]*)?$")]
    private static partial Regex GetUrlRegex();

    // Generated regex for validating emails
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex GetEmailRegex();
}
