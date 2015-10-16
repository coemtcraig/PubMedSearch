using System.Threading.Tasks;

namespace RSS.PubMed
{
    internal abstract class ClientBase : HttpClientWrapperBase
    {
        protected string SearchType;

        protected ClientBase(string baseUrl, string searchType) : base(baseUrl)
        {
            SearchType = searchType;
        }

        protected async Task<T> Execute<T>(string searchParameters)
        {
            return await ExecuteSearch<T>(SearchType, searchParameters);
        }

        protected async Task<string> ExecuteXml(string searchParameters)
        {
            return await ExecuteXml(SearchType, searchParameters);
        }
    }
}