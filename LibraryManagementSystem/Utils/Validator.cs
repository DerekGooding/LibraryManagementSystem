using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Utils;

internal static partial class Validator
{
    public static bool IsURL(ref string url)
    {
        url = url.Trim();

        return !string.IsNullOrWhiteSpace(url) && GetUrlRegex().IsMatch(url);
    }

    public static bool IsEmail(ref string email)
    {
        email = email.Trim().ToLower();

        return !string.IsNullOrWhiteSpace(email) && GetEmailRegex().IsMatch(email);
    }

    // Generated regex for validating URLs
    [GeneratedRegex(@"^https?:\/\/([a-zA-Z0-9-\.]+)(\.[a-zA-Z]{2,})(\:\d+)?(\/[\w\-\.~]*)*(\?[;&a-zA-Z0-9\-\.=_~%]*)?(\#[-a-zA-Z0-9_]*)?$")]
    private static partial Regex GetUrlRegex();

    // Generated regex for validating emails
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex GetEmailRegex();
}