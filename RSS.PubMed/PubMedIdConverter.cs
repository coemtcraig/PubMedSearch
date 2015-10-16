using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RSS.PubMed
{
    internal class PubMedIdConverter
    {
        private readonly PubMedConfiguration configuration;

        public PubMedIdConverter()
        {
            configuration = new PubMedConfiguration();
        }

        public async Task<string> GetPmcidAsync(string pmid)
        {
            var pmcid = "";
            var root = await GetDocumentRoot(pmid);

            if (root != null)
            {
                var record = root.GetElement("record", pmid);

                var statusAttribute = record.Attributes("status").FirstOrDefault();
                if (statusAttribute != null && statusAttribute.Value == "error") return pmcid;

                pmcid = record.GetAttribute("pmcid", pmid).Value;
            }

            return pmcid;
        }

        public async Task<List<IdMapping>> GetPmcidsAsync(string pmidList)
        {
            var idMappings = new List<IdMapping>();
            var root = await GetDocumentRoot(pmidList);

            if (root != null)
            {
                var records = root.Elements("record");
                foreach (var record in records)
                {
                    try
                    {
                        var pmid = record.Attributes("pmid").FirstOrDefault();
                        var pmcid = record.Attributes("pmcid").FirstOrDefault();

                        var mapping = new IdMapping
                        {
                            Pmid = pmid != null ? pmid.Value : "",
                            Pmcid = pmcid != null ? pmcid.Value : ""
                        };

                        idMappings.Add(mapping);
                    }
                    catch
                    {
                        var pmid = record.Attributes("pmid").FirstOrDefault();

                        idMappings.Add(new IdMapping {Pmid = pmid != null ? pmid.Value : "", Pmcid = ""});
                    }
                }
            }

            return idMappings;
        }

        private async Task<XElement> GetDocumentRoot(string pmid)
        {
            var parameters = GenerateSearchParameters(pmid);
            var idConverterClient = new IdConverterClient(configuration.IdConverterBaseUrl);
            var xml = await idConverterClient.ExecuteXml(parameters);

            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;

            return root;
        }

        private string GenerateSearchParameters(string id)
        {
            var query = new StringBuilder();
            query.AppendFormat("ids={0}", id);
            query.Append("&versions=no");
            query.AppendFormat("&tool={0}", configuration.ApplicationName);
            query.AppendFormat("&email={0}", ClientHelper.UrlSafe(configuration.Email));
            return query.ToString();
        }
    }
}