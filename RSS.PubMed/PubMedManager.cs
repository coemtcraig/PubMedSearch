using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RSS.PubMed.Interfaces;
using RSS.PubMed.Models;

namespace RSS.PubMed
{
    public class PubMedManager : IPubMedManager
    {
        private readonly PubMedConfiguration configuration;

        public PubMedManager()
        {
            configuration = new PubMedConfiguration();
        }

        public async Task<ESearch> SearchPubMedAsync(string term, string termType, DateTime? startDate, DateTime? endDate)
        {
            var pubMedSearch = new PubMedSearch();
            return await pubMedSearch.SearchAsync(term, termType, startDate, endDate);
        }

        public async Task<PubMedArticle> FetchPubMedArticleAsync(string pubMedId)
        {
            var pubMedFetch = new PubMedFetch();
            return await pubMedFetch.GetPubMedArticleAsync(pubMedId);
        }

        public async Task<string> FetchPubMedArticleRawAsync(string pubMedId)
        {
            var pubMedFetch = new PubMedFetch();
            return await pubMedFetch.GetPubMedArticleRawAsync(pubMedId);
        }

        public async Task<List<PubMedArticle>> FetchPubMedArticlesAsync(string pubMedIdList)
        {
            var pubMedFetch = new PubMedFetch();
            return await pubMedFetch.GetPubMedArticlesAsync(pubMedIdList);
        }

        public string GetFullArticleUrl(string pmcid)
        {
            if (pmcid.IsNullOrEmpty()) return null;

            return $"{configuration.FullArticleBaseUrl}{pmcid}/";
        }
    }
}