using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSS.PubMed.Models
{
    public class ESearch
    {
        public Header Header { get; set; }

        [JsonProperty("esearchresult")]
        public ESearchResult ESearchResult { get; set; }
    }

    public class Header
    {
        public string Type { get; set; }
        public string Version { get; set; }
    }

    public class ESearchResult
    {
        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("idlist")]
        public List<string> IdList { get; set; }
    }
}