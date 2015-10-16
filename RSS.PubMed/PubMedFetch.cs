using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RSS.PubMed.Models;

namespace RSS.PubMed
{
    internal class PubMedFetch
    {
        private readonly PubMedConfiguration configuration;
        private readonly PubMedIdConverter pubMedIdConverter;

        public PubMedFetch()
        {
            configuration = new PubMedConfiguration();
            pubMedIdConverter = new PubMedIdConverter();
        }

        public async Task<string> GetPubMedArticleRawAsync(string pmid)
        {
            var parameters = GenerateSearchParameters(pmid);
            var efetchClient = new EFetchClient(configuration.BaseUrl);
            var xml = await efetchClient.ExecuteXml(parameters);

            return xml;
        }

        public async Task<PubMedArticle> GetPubMedArticleAsync(string pmid)
        {
            PubMedArticle publication = null;
            var parameters = GenerateSearchParameters(pmid);
            var efetchClient = new EFetchClient(configuration.BaseUrl);
            var xml = await efetchClient.ExecuteXml(parameters);

            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;

            if (root != null)
            {
                publication = GetPublication(root.GetElement("PubmedArticle", pmid), pmid);
                publication.Pmcid = await pubMedIdConverter.GetPmcidAsync(pmid);
            }

            return publication;
        }

        public async Task<List<PubMedArticle>> GetPubMedArticlesAsync(string pmidList)
        {
            var publicationList = new List<PubMedArticle>();
            var parameters = GenerateSearchParameters(pmidList);
            var efetchClient = new EFetchClient(configuration.BaseUrl);
            var xml = await efetchClient.ExecuteXml(parameters);

            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;

            if (root == null) return publicationList;

            foreach (var pubmedArticle in root.Elements("PubmedArticle"))
            {
                publicationList.Add(GetPublication(pubmedArticle, ""));
            }

            var idMappings = await pubMedIdConverter.GetPmcidsAsync(pmidList);
            foreach (var mapping in idMappings)
            {
                if (!mapping.Pmcid.IsNullOrEmpty())
                {
                    var article = publicationList.FirstOrDefault(x => x.PubMedId == mapping.Pmid);
                    if (article != null)
                    {
                        article.Pmcid = mapping.Pmcid;
                    }
                }
            }

            return publicationList;
        }

        private PubMedArticle GetPublication(XElement pubmedArticle, string pmid)
        {
            var publication = new PubMedArticle();

            var medlineCitation = pubmedArticle.GetElement("MedlineCitation", pmid);
            publication.PubMedId = medlineCitation.GetElement("PMID", pmid).Value;
            if (pmid.IsNullOrEmpty()) pmid = publication.PubMedId;

            var article = medlineCitation.GetElement("Article", pmid);
            var journal = article.GetElement("Journal", pmid);
            var issnElement = journal.GetElement("ISSN", pmid);
            var history = pubmedArticle.GetElement("PubmedData/History", pmid);

            publication.Issn = issnElement.Value;
            publication.IssnType = issnElement.GetAttribute("IssnType", pmid).Value;
            publication.Volume = journal.GetOptionalElementValue("JournalIssue/Volume");
            publication.Issue = journal.GetOptionalElementValue("JournalIssue/Issue");
            publication.PublicationMonth = journal.GetOptionalElementValue("JournalIssue/PubDate/Month");
            publication.PublicationYear = journal.GetOptionalElementValue("JournalIssue/PubDate/Year");
            publication.JournalTitle = journal.GetOptionalElementValue("Title");
            publication.IsoAbbreviation = journal.GetOptionalElementValue("ISOAbbreviation");
            publication.ArticleTitle = article.GetOptionalElementValue("ArticleTitle");
            publication.MedlinePgn = article.GetOptionalElementValue("Pagination/MedlinePgn");
            publication.Abstract = article.GetOptionalElementValue("Abstract/AbstractText");
            publication.Authors = GetAuthors(article.GetElement("AuthorList", pmid));
            publication.PublicationTypes = GetPublicationTypes(article.GetElement("PublicationTypeList", pmid));

            var pubMedPublishInfo = GetPubMedDate(history.GetElement("PubMedPubDate[@PubStatus='pubmed']", pmid), pmid);
            publication.PubMedPublishYear = pubMedPublishInfo.Item1;
            publication.PubMedPublishMonth = pubMedPublishInfo.Item2;
            publication.PubMedPublishDay = pubMedPublishInfo.Item3;

            var entrezPublishInfo = GetPubMedDate(history.GetElement("PubMedPubDate[@PubStatus='entrez']", pmid), pmid);
            publication.EntrezPublishYear = entrezPublishInfo.Item1;
            publication.EntrezPublishMonth = entrezPublishInfo.Item2;
            publication.EntrezPublishDay = entrezPublishInfo.Item3;

            var medlinePublishInfo = GetPubMedDate(history.GetElement("PubMedPubDate[@PubStatus='medline']", pmid), pmid);
            publication.MedlinePublishYear = medlinePublishInfo.Item1;
            publication.MedlinePublishMonth = medlinePublishInfo.Item2;
            publication.MedlinePublishDay = medlinePublishInfo.Item3;

            var accessionNumberList = article.GetOptionalElement("DataBankList/DataBank/AccessionNumberList");
            if (accessionNumberList != null)
                publication.NctNumbers = GetAccessionNumbers(accessionNumberList);

            return publication;
        }

        private List<PubMedAuthor> GetAuthors(XElement authorList)
        {
            return authorList.Elements("Author").Select(GetAuthor).ToList();
        }

        private PubMedAuthor GetAuthor(XElement authorElement)
        {
            var author = new PubMedAuthor
            {
                LastName = authorElement.GetOptionalElementValue("LastName"),
                ForeName = authorElement.GetOptionalElementValue("ForeName"),
                Initials = authorElement.GetOptionalElementValue("Initials"),
                Affiliation = authorElement.GetOptionalElementValue("AffiliationInfo/Affiliation")
            };

            return author;
        }

        private List<string> GetAccessionNumbers(XElement accessionNumberList)
        {
            return accessionNumberList.Elements("AccessionNumber").Select(element => element.Value).ToList();
        }

        private List<PubMedPublicationType> GetPublicationTypes(XElement publicationTypeList)
        {
            return publicationTypeList?.Elements("PublicationType").Select(element => new PubMedPublicationType { Name = element.Value }).ToList();
        }

        private Tuple<int, int, int> GetPubMedDate(XElement pubMedPubDate, string pmid)
        {
            var year = Convert.ToInt32(pubMedPubDate.GetElement("Year", pmid).Value);
            var month = Convert.ToInt32(pubMedPubDate.GetElement("Month", pmid).Value);
            var day = Convert.ToInt32(pubMedPubDate.GetElement("Day", pmid).Value);

            return new Tuple<int, int, int>(year, month, day);
        }

        private string GenerateSearchParameters(string id)
        {
            var query = new StringBuilder();
            query.AppendFormat("db={0}", configuration.Database);
            query.AppendFormat("&tool={0}", configuration.ApplicationName);
            query.AppendFormat("&email={0}", ClientHelper.UrlSafe(configuration.Email));
            query.AppendFormat("&retmode=xml");
            query.AppendFormat("&id={0}", id);
            return query.ToString();
        }
    }
}