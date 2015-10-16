using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSS.PubMed.Models;

namespace RSS.PubMed.Tests
{
    [TestClass]
    public class PubMedFetchTests : TestBase
    {
        [TestMethod]
        public async Task GetPublicationByPMID()
        {
            var publication = await Manager.FetchPubMedArticleAsync("23265711");
            PublicationAssertions(publication);
        }

        [TestMethod]
        public async Task GetPublicationsByPMIDList()
        {
            var publications = await Manager.FetchPubMedArticlesAsync("23265711");
            var publication = publications[0];

            Assert.IsTrue(publications.Count == 1);
            PublicationAssertions(publication);
        }

        [TestMethod]
        public async Task CheckSinglePmcid()
        {
            var publication = await Manager.FetchPubMedArticleAsync("21807251");
            Assert.AreEqual(publication.Pmcid, "PMC3150462");
        }

        [TestMethod]
        public async Task GetMultiplePmcids()
        {
            var pmids = "21868505,21807251,21766019,21757680,11748933";
            var articles = await Manager.FetchPubMedArticlesAsync(pmids);

            Assert.AreEqual("PMC3262032", articles.First(x => x.PubMedId == "21868505").Pmcid);
            Assert.AreEqual("PMC3150462", articles.First(x => x.PubMedId == "21807251").Pmcid);
            Assert.AreEqual("PMC3135221", articles.First(x => x.PubMedId == "21766019").Pmcid);
            Assert.AreEqual("PMC3262662", articles.First(x => x.PubMedId == "21757680").Pmcid);
            Assert.AreEqual(null, articles.First(x => x.PubMedId == "11748933").Pmcid);
        }

        [TestMethod]
        public async Task PmcidNotFound()
        {
            var publication = await Manager.FetchPubMedArticleAsync("11748933");
            Assert.IsTrue(publication.Pmcid.IsNullOrEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Exceptions.PubMedElementNotFoundException))]
        public async Task GetPublicationByPMIDElementNotFound()
        {
            await Manager.FetchPubMedArticleAsync("9999999999999999999999999999999999999999999999999999999999999");
        }

        [TestMethod]
        public void GenerateFullArticleUrl()
        {
            var url = Manager.GetFullArticleUrl("PMC3262032");
            Assert.AreEqual("http://www.ncbi.nlm.nih.gov/pmc/articles/PMC3262032/", url);
        }

        private void PublicationAssertions(PubMedArticle article)
        {
            Assert.AreEqual("23265711", article.PubMedId);
            Assert.AreEqual("1879-0852", article.Issn);
            Assert.AreEqual("Electronic", article.IssnType);
            Assert.AreEqual("49", article.Volume);
            Assert.AreEqual("6", article.Issue);
            Assert.AreEqual("2013", article.PublicationYear);
            Assert.AreEqual("Apr", article.PublicationMonth);
            Assert.AreEqual("European journal of cancer (Oxford, England : 1990)", article.JournalTitle);
            Assert.AreEqual("Eur. J. Cancer", article.IsoAbbreviation);
            Assert.IsTrue(!article.ArticleTitle.IsNullOrEmpty());
            Assert.AreEqual("1161-8", article.MedlinePgn);
            Assert.IsTrue(!article.Abstract.IsNullOrEmpty());
            Assert.AreEqual(11, article.Authors.Count);
            Assert.AreEqual(4, article.PublicationTypes.Count);
            Assert.AreEqual(2012, article.PubMedPublishYear);
            Assert.AreEqual(12, article.PubMedPublishMonth);
            Assert.AreEqual(26, article.PubMedPublishDay);
            Assert.AreEqual(2012, article.EntrezPublishYear);
            Assert.AreEqual(12, article.EntrezPublishMonth);
            Assert.AreEqual(26, article.EntrezPublishDay);
            Assert.AreEqual(2013, article.MedlinePublishYear);
            Assert.AreEqual(5, article.MedlinePublishMonth);
            Assert.AreEqual(29, article.MedlinePublishDay);

            Assert.AreEqual(2, article.NctNumbers.Count);
            Assert.AreEqual("NCT00122460", article.NctNumbers[0]);
            Assert.AreEqual("NCT00154102", article.NctNumbers[1]);
        }
    }
}