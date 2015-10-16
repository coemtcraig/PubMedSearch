namespace RSS.PubMed
{
    internal static class ClientHelper
    {
        public static string UrlSafe(string urlValue)
        {
            if (!urlValue.IsNullOrEmpty())
                return System.Uri.EscapeDataString(urlValue);

            return "";
        }
    }
}