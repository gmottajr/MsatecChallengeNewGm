namespace MsaTec.Core.Extensions;

public static class StringExtensions
{
    public static string SafeToString(this string value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public static bool IsSameText(this string value, string comparing)
    {
        return value.SafeToString().ToLower() == comparing.SafeToString().ToLower();
    }

    public static string SafeTrim(this string value)
    {
        return value.SafeToString().Trim();
    }

    public static string SafeToUpper(this string value)
    {
        return value.SafeToString().ToUpper();
    }

    public static string SafeToLower(this string value)
    {
        return value.SafeToString().ToLower();
    }

    public static bool IsEmpty(this string value)
    {
        return !(value.SafeTrim().Length > 0);
    }
}
