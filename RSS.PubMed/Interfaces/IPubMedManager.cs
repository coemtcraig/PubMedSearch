using System;
using System.Threading.Tasks;
using RSS.PubMed.Models;
using System.Collections.Generic;

namespace RSS.PubMed.Interfaces
{
    public interface IPubMedManager
    {
        Task<ESearch> SearchPubMedAsync(string term, string termType, DateTime? startDate, DateTime? endDate);
        Task<PubMedArticle> FetchPubMedArticleAsync(string pubMedId);
        Task<string> FetchPubMedArticleRawAsync(string pubMedId);
        Task<List<PubMedArticle>> FetchPubMedArticlesAsync(string pubMedIdList);
        string GetFullArticleUrl(string pmcid);
    }
}