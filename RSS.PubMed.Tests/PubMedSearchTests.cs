using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RSS.PubMed.Tests
{
    [TestClass]
    public class PubMedSearchTests : TestBase
    {
        [TestMethod]
        public async Task ExecuteAuthorSearch()
        {
            var searchResult = await Manager.SearchPubMedAsync("smith", "author", null, null);

            Assert.AreEqual("esearch", searchResult.Header.Type);
            Assert.IsTrue(searchResult.ESearchResult.IdList.Count == 20);
        }

        [TestMethod]
        public async Task ExecuteTitleSearch()
        {
            var pmid = "26374631";
            var title = "Experiences of healing therapy in patients with irritable bowel syndrome";
            var searchResult = await Manager.SearchPubMedAsync(title, "title", null, null);

            Assert.AreEqual("esearch", searchResult.Header.Type);
            Assert.AreEqual(2, searchResult.ESearchResult.IdList.Count);
            Assert.IsTrue(searchResult.ESearchResult.IdList.Contains(pmid));
        }
    }
}