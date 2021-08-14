using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp.Entities
{
    public class SearchResult
    {
        public string SEOTerm { get; set; }
        public string URL { get; set; }
        public int Hits { get; set; }
        public string Body { get; set; }
    }
}
