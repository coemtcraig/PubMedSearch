using System.Collections.Generic;

namespace RSS.PubMed.Models
{
    public class PubMedArticle
    {
        public string PubMedId { get; set; }
        public string Issn { get; set; }
        public string IssnType { get; set; }
        public string PublicationMonth { get; set; }
        public string PublicationYear { get; set; }
        public string JournalTitle { get; set; }
        public string IsoAbbreviation { get; set; }
        public string Volume { get; set; }
        public string Issue { get; set; }
        public string ArticleTitle { get; set; }
        public string MedlinePgn { get; set; }
        public string Abstract { get; set; }
        public int PubMedPublishYear { get; set; }
        public int PubMedPublishMonth { get; set; }
        public int PubMedPublishDay { get; set; }
        public int EntrezPublishYear { get; set; }
        public int EntrezPublishMonth { get; set; }
        public int EntrezPublishDay { get; set; }
        public int MedlinePublishYear { get; set; }
        public int MedlinePublishMonth { get; set; }
        public int MedlinePublishDay { get; set; }
        public int SearchQueueId { get; set; }
        public string Pmcid { get; set; }
        public List<string> NctNumbers { get; set; }

        public List<PubMedAuthor> Authors { get; set; }
        public List<PubMedPublicationType> PublicationTypes { get; set; }
    }
}