using System;
using System.Text;
using System.Threading.Tasks;
using RSS.PubMed.Models;

namespace RSS.PubMed
{
    internal class PubMedSearch
    {
        private readonly PubMedConfiguration configuration;

        public PubMedSearch()
        {
            configuration = new PubMedConfiguration();
        }

        internal async Task<ESearch> SearchAsync(string term, string termType, DateTime? startDate, DateTime? endDate)
        {
            var parameters = GenerateSearchParameters(term, termType, startDate, endDate);
            var esearchClient = new ESearchClient(configuration.BaseUrl);
            return await esearchClient.ExecuteSearch(parameters);
        }

        private string GenerateSearchParameters(string searchTerm, string termType, DateTime? startDate, DateTime? endDate)
        {
            var query = new StringBuilder();
            query.AppendFormat("db={0}", configuration.Database);
            query.AppendFormat("&dbFrom={0}", configuration.Database);
            query.AppendFormat("&term={0}", ClientHelper.UrlSafe($"{searchTerm}[{termType}]"));
            query.AppendFormat("&tool={0}", configuration.ApplicationName);
            query.AppendFormat("&email={0}", ClientHelper.UrlSafe(configuration.Email));
            query.AppendFormat("&retmode=json");

            if (startDate.HasValue || endDate.HasValue)
                query.AppendFormat("&dateType=pdat");

            if (startDate.HasValue)
                query.AppendFormat("&mindate={0}", startDate.Value.ToString("yyyy/MM/dd"));

            if (endDate.HasValue)
                query.AppendFormat("&maxdate={0}", endDate.Value.ToString("yyyy/MM/dd"));

            return query.ToString();
        }
    }
}