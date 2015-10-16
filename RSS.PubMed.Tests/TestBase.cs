using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RSS.PubMed.Tests
{
    public abstract class TestBase
    {
        protected PubMedManager Manager;

        [TestInitialize]
        public void Initialize()
        {
            Manager = new PubMedManager();
        }
    }
}