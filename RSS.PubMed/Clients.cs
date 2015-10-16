using System.Threading.Tasks;
using RSS.PubMed.Models;

namespace RSS.PubMed
{
    internal class ESearchClient : ClientBase
    {
        public ESearchClient(string baseUrl) : base(baseUrl, "esearch.fcgi") { }

        public async Task<ESearch> ExecuteSearch(string searchParameters)
        {
            return await Execute<ESearch>(searchParameters);
        }
    }

    internal class EFetchClient : ClientBase
    {
        public EFetchClient(string baseUrl) : base(baseUrl, "efetch.fcgi") { }

        public new async Task<string> ExecuteXml(string searchParameters)
        {
            return await base.ExecuteXml(searchParameters);
        }
    }

    internal class IdConverterClient : ClientBase
    {
        public IdConverterClient(string baseUrl) : base(baseUrl, "") { }

        public new async Task<string> ExecuteXml(string searchParameters)
        {
            return await base.ExecuteXml(searchParameters);
        }
    }
}