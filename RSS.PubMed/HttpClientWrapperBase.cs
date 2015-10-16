using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RSS.PubMed
{
    internal abstract class HttpClientWrapperBase : IDisposable
    {
        private readonly string baseUrl;
        private HttpClient httpClient;

        protected HttpClientWrapperBase(string baseUrl)
        {
            this.baseUrl = baseUrl;
            httpClient = new HttpClient();
        }

        protected HttpClient Client()
        {
            if (httpClient == null)
                throw new ObjectDisposedException("HttpClient has been disposed");

            return httpClient;
        }

        protected async Task<T> ExecuteSearch<T>(string searchType, string searchParameters)
        {
            var response = await Client().GetStringAsync(baseUrl + searchType + "?" + searchParameters);
            return JsonConvert.DeserializeObject<T>(response);
        }

        protected async Task<string> ExecuteXml(string searchType, string searchParameters)
        {
            var xml = await Client().GetStringAsync(baseUrl + searchType + "?" + searchParameters);
            return xml;
        }

        #region Dispose
        ~HttpClientWrapperBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (httpClient != null)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }
            }
        }
        #endregion
    }
}