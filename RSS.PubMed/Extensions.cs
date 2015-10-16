using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RSS.PubMed
{
    internal static class Extensions
    {
        public static XElement GetElement(this XElement value, string xpath, string pmid)
        {
            var element = value.XPathSelectElement(xpath);
            if (element == null)
                throw new Exceptions.PubMedElementNotFoundException($"Element {xpath} for pubmed id {pmid} is null");

            return element;
        }

        public static XElement GetOptionalElement(this XElement value, string xpath)
        {
            return value.XPathSelectElement(xpath);
        }

        public static string GetOptionalElementValue(this XElement value, string xpath)
        {
            var element = value.XPathSelectElement(xpath);
            return element?.Value ?? "";
        }

        public static XAttribute GetAttribute(this XElement value, string attributeName, string pmid)
        {
            var attribute = value.Attributes(attributeName).FirstOrDefault();
            if (attribute == null)
                throw new Exceptions.PubMedAttributeNotFoundException($"Attribute {attributeName} for pubmed id {pmid} is null");

            return attribute;
        }
    }

    public static class ExternalExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
