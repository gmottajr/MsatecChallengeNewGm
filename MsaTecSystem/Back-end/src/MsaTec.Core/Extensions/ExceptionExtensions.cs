using System.Text;


namespace MsaTec.Core.Extensions;

public static class ExceptionExtensionsShopMix
{
    public static string ExtractInfoAll(this Exception value)
    {
        return ExtractInfoRecursively(value).ToString();
    }

    private static StringBuilder ExtractInfoRecursively(Exception ex, StringBuilder strBuilder = null)
    {
        if (ex.InnerException != null)
            strBuilder = ExtractInfoRecursively(ex.InnerException, strBuilder);

        if (strBuilder == null)
            strBuilder = new StringBuilder();
        strBuilder.Append($"Exception: {ex.GetType()}.\nMessage: {ex.Message}.\nStack Trace: {ex.StackTrace};");
        strBuilder.Append("\n\n");

        return strBuilder;
    }
}
